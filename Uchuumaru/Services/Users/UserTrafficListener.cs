using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;
using Uchuumaru.Notifications.Users;

namespace Uchuumaru.Services.Users
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the <see cref="UserJoinedNotification"/> and
    /// <see cref="UserLeftNotification"/>.
    /// </summary>
    public class UserTrafficListener : INotificationHandler<UserJoinedNotification>, INotificationHandler<UserLeftNotification>
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="UserListener"/> with the
        /// given injected dependencies.
        /// </summary>
        public UserTrafficListener(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <summary>
        /// Sends the a traffic message to the Guild traffic channel when a user joins.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(UserJoinedNotification notification, CancellationToken cancellationToken)
        {
            var user = notification.User;
            var channel = await GetTrafficChannel(user.Guild);

            if (channel is null)
            {
                return;
            }

            await SendTraffic("User Joined", user, Color.Green, channel);
        }

        /// <summary>
        /// Sends the a traffic message to the Guild traffic channel when a user leaves.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(UserLeftNotification notification, CancellationToken cancellationToken)
        {
            var user = notification.User;
            var channel = await GetTrafficChannel(user.Guild);

            if (channel is null)
            {
                return;
            }

            await SendTraffic("User Left", user, Color.Red, channel);
        }

        /// <summary>
        /// Builds and sends a traffic message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="user">The user.</param>
        /// <param name="color">The colour.</param>
        /// <param name="channel">The traffic channel.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private static async Task SendTraffic(
            string title, 
            IUser user, 
            Color color, 
            IMessageChannel channel)
        {
            var embed = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithTitle(title)
                .WithColor(color)
                .AddField("User", $"{user} ({user.Id})")
                .Build();

            await channel.SendMessageAsync(embed: embed);
        }
        
        /// <summary>
        /// Gets a Guild's traffic channel.
        /// </summary>
        /// <param name="socketGuild">The Guild.</param>
        /// <returns>
        /// An <see cref="IMessageChannel"/> representation of the Guild's traffic
        /// channel.
        /// </returns>
        private async Task<IMessageChannel> GetTrafficChannel(SocketGuild socketGuild)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == socketGuild.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            _ = guild ?? throw new EntityNotFoundException<Guild>();

            if (guild.TrafficChannelId == 0)
            {
                return null;
            }

            return socketGuild.GetChannel(guild.TrafficChannelId) as IMessageChannel;
        }
    }
}