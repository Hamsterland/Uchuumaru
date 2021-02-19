using System;
using System.Collections.Generic;
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
            var message = notification.Message;
            var author = message.Author as IGuildUser;
            var guild = (message.Channel as IGuildChannel).Guild;

            if (author.Id == _client.CurrentUser.Id)
                return;
            
            if (author.GuildPermissions.Has(GuildPermission.ManageMessages))
                return;

            var status = await _filter.GetFilterStatus(guild.Id);

            if (!status.Enabled)
                return;
            
            var expressions = status.Expressions;

            var regexes = expressions
                .Select(expression => new Regex(expression))
                .ToList();

            if (regexes.Any(regex => regex.IsMatch(message.Content)))
            {
                var matches = regexes
                    .Where(regex => regex.IsMatch(message.Content))
                    .Select(x => x.Matches(message.Content))
                    .ToArray();
                
                await message.DeleteAsync();
                await LogViolation(status, message, guild, matches);
                await CreateFilterInfraction(author.Id, _client.CurrentUser.Id, guild.Id);
            }
        }
        
        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static async Task LogViolation(FilterStatus status, SocketMessage message, IGuild guild, MatchCollection[] matchCollections)
        {
            if (await guild.GetChannelAsync(status.FilterChannelId) is not ITextChannel filterChannel)
                return;

            var sb = new StringBuilder();
            foreach (var collection in matchCollections)
                sb.AppendJoin(", ", collection.Select(x => x.Value));
            
            var embed = new EmbedBuilder()
                .WithTitle("Filter Violation")
                .WithColor(Constants.DefaultColour)
                .AddField("Author", message.Author.Represent())
                .AddField("Channel", message.Channel.Represent())
                .AddField("Violations", sb)
                .Build();

            await filterChannel?.SendMessageAsync(embed: embed);
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
    }
}