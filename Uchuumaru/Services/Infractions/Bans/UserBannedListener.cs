using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Uchuumaru.Data.Models;
using Uchuumaru.Notifications.User;

namespace Uchuumaru.Services.Infractions.Bans
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the
    /// <see cref="UserBannedNotification"/>.
    /// </summary>
    public class UserBannedListener : INotificationHandler<UserBannedNotification>
    {
        /// <summary>
        /// The infraction service.
        /// </summary>
        private readonly IInfractionService _infraction;

        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="UserBannedListener"/> with the given
        /// injected dependencies,
        /// </summary>
        public UserBannedListener(IInfractionService infraction, DiscordSocketClient client)
        {
            _infraction = infraction;
            _client = client;
        }

        public async Task Handle(UserBannedNotification notification, CancellationToken cancellationToken)
        {
            var user = notification.User;
            var guild = notification.Guild;

            var id = await _infraction.CreateInfraction(
                InfractionType.Ban,
                guild.Id,
                user.Id,
                0,
                TimeSpan.Zero);
            
            var infractionChannelId = await _infraction.GetInfractionChannelId(guild.Id);

            // The infraction channel was never set.
            if (infractionChannelId == 0)
            {
                return;  
            }

            // ReSharper disable once UseNegatedPatternMatching
            var infractionChannel = _client.GetChannel(infractionChannelId) as ITextChannel;

            // The infraction channel does not exist or it is not an ITextChannel.
            if (infractionChannel is null)
            {
                return; 
            }

            var builder = new InfractionEmbedBuilder("User Banned", id, user);
            var message = await infractionChannel.SendMessageAsync(embed: builder.Build());

            builder.Moderator = $"+infraction claim {message.Id}";
            builder.Reason = $"+infraction reason {message.Id} <reason>";
            
            await message.ModifyAsync(props => props.Embed = builder.Build());
        }
    }
}