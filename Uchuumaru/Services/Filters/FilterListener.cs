﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Uchuumaru.Data.Models;
using Uchuumaru.Notifications.Message;
using Uchuumaru.Services.Infractions;

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
        public FilterListener(IFilterService filter, DiscordSocketClient client, IInfractionService infraction)
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
            {
                return;
            }
            
            if (author.GuildPermissions.Has(GuildPermission.ManageMessages))
            {
                return; 
            }

            var status = await _filter.GetFilterStatus(guild.Id);
            var expressions = status.Expressions;

            var regexes = expressions
                .Select(expression => new Regex(expression))
                .ToList();

            if (regexes.Any(regex => regex.IsMatch(message.Content)))
            {
                await message.DeleteAsync();
                await LogViolation(status, message, guild);
                await CreateFilterInfraction(author.Id, _client.CurrentUser.Id, guild.Id);
            }
        }

        /// <summary>
        /// Logs a violation of a filter to the filter log. 
        /// </summary>
        /// <param name="status">The filter status.</param>
        /// <param name="message">The filtered message.</param>
        /// <param name="guild">The guild.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private static async Task LogViolation(FilterStatus status, SocketMessage message, IGuild guild)
        {
            var filterChannel = await guild.GetChannelAsync(status.FilterChannelId) as ITextChannel;

            var embed = new EmbedBuilder()
                .WithTitle("Filter Violation")
                .AddField("Author", $"{message.Author} ({message.Author.Id})")
                .AddField("Channel", $"{message.Channel.Name} ({message.Channel.Id})")
                .WithDescription(Format.Sanitize(message.Content))
                .Build();

            await filterChannel?.SendMessageAsync(embed: embed);
        }

        /// <summary>
        /// Creates a filter violation infraction.
        /// </summary>
        /// <remarks>
        /// The <paramref name="moderatorId"/> in this case is always the Id
        /// of the current client user.
        /// </remarks>
        /// <param name="subjectId">The Id of the user who violated the filter.</param>
        /// <param name="moderatorId">The moderator overseeing the infraction.</param>
        /// <param name="guildId">The guild the violation occured in.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion. 
        /// </returns>
        private async Task CreateFilterInfraction(ulong subjectId, ulong moderatorId, ulong guildId)
        {
            await _infraction.CreateInfraction(
                InfractionType.Filter,
                guildId,
                subjectId,
                moderatorId,
                TimeSpan.Zero,
                "Filter Violation"
            );
        }
    }
}