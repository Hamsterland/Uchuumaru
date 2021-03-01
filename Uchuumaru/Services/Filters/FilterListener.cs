using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Serilog;
using Uchuumaru.Data.Models;
using Uchuumaru.Notifications.Message;
using Uchuumaru.Services.Infractions;
using Uchuumaru.Utilities;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the <see cref="MessageReceivedNotification"/>.
    /// </summary>
    public class FilterListener : INotificationHandler<MessageReceivedNotification>
    {
        /// <summary>
        /// The filter service.
        /// </summary>
        private readonly IFilterService _filter;

        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// The infraction service.
        /// </summary>
        private readonly IInfractionService _infraction;
        

        /// <summary>
        /// Constructs a new <see cref="FilterListener"/> with the given
        /// injected dependencies.
        /// </summary>
        public FilterListener(IFilterService filter, DiscordSocketClient client, IInfractionService infraction, ILogger logger)
        {
            _filter = filter;
            _client = client;
            _infraction = infraction;
        }

        /// <summary>
        /// Determines if the content of the received <paramref name="notification.Message"/> violates the filter. The
        /// check is skipped if the message author has the <see cref="GuildPermissions.ManageMessages"/> permission.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.Message.Channel is IDMChannel)
                return;
            
            var message = notification.Message;
            var author = message.Author as IGuildUser;
            var guild = (message.Channel as IGuildChannel).Guild;

            if (author.Id == _client.CurrentUser.Id)
                return;
            
            var status = await _filter.GetFilterStatus(guild.Id);

            // Hotfix to make testing the filter possible 
            if (!message.Content.StartsWith("+filter test"))
            {
                if (author.GuildPermissions.Has(GuildPermission.ManageMessages))
                    return;
            }
            
            if (!status.Enabled)
                return;
            
            var expressions = status.Expressions;

            var regexes = expressions
                .Select(expression => new Regex(expression, RegexOptions.IgnoreCase))
                .ToList();

            if (regexes.Any(regex => regex.IsMatch(message.Content)))
            {
                var matches = regexes
                    .Where(regex => regex.IsMatch(message.Content))
                    .Select(x => x.Matches(message.Content))
                    .ToArray();
                
                var sb = new StringBuilder();
                foreach (var collection in matches)
                    sb.AppendJoin(", ", collection.Select(x => x.Value));
                var violations = sb.ToString();
                
                await message.DeleteAsync();
                await LogViolation(status, message, violations);
                await PrivateMessageUser(message.Author, guild, message.Channel, violations);
                await CreateFilterInfraction(author.Id, _client.CurrentUser.Id, guild.Id);
            }
        }
        
        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private async Task LogViolation(FilterStatus status, SocketMessage message, string violations)
        {
            if (_client.GetChannel(status.FilterChannelId) is not ITextChannel filterChannel)
                return;
            
            var embed = new EmbedBuilder()
                .WithTitle("Filter Violation")
                .WithColor(Constants.DefaultColour)
                .WithDescription(message.Content)
                .AddField("Author", message.Author.Represent())
                .AddField("Channel", message.Channel.Represent())
                .AddField("Violations", violations)
                .Build();

            await filterChannel.SendMessageAsync(embed: embed);
        }
        
        private async Task CreateFilterInfraction(ulong subjectId, ulong moderatorId, ulong guildId)
        {
            await _infraction.CreateInfraction(
                InfractionType.FILTER,
                guildId,
                subjectId,
                moderatorId,
                TimeSpan.Zero,
                "Filter Violation"
            );
        }

        private static async Task PrivateMessageUser(IUser user, IGuild guild, IChannel channel, string violations)
        {
            
            var message = $"{user.Represent()}, your message in #{channel.Represent()} in the server {guild.Name} " +
                          $"was deleted because it contained the following disallowed content: {violations}. " +
                          "Please refrain from sending this content again. " +
                          "Notify a moderator if you have any further queries or believe this was a mistake.";

            await user?.SendMessageAsync(message);
        }
    }
}