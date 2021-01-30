using System;
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

        private async Task SendTraffic(
            string title, 
            IUser user, 
            Color color, 
            IMessageChannel channel)
        {
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(color)
                .AddField("User", $"{user} ({user.Id})")
                .Build();

            await channel.SendMessageAsync(embed: embed);
        }
        
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