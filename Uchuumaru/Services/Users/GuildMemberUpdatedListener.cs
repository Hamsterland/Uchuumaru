using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the
    /// <see cref="GuildMemberUpdatedNotification"/>.
    /// </summary>
    public class GuildMemberUpdatedListener : INotificationHandler<GuildMemberUpdatedNotification>
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="GuildMemberUpdatedListener"/> with
        /// the given injected dependencies.
        /// </summary>
        public GuildMemberUpdatedListener(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <summary>
        /// Initiates the sequence of checks to be carried out on this
        /// event.
        /// 1) Determines if the nickname has changed.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(GuildMemberUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var before = notification.Before;
            var after = notification.After;
            await CompareNicknames(before, after);
        }
        
        /// <summary>
        /// Determines if the nickname has changed.
        /// </summary>
        /// <param name="before">The user before update.</param>
        /// <param name="after">The user after update.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task CompareNicknames(SocketGuildUser before, SocketGuildUser after)
        {
            if (before.Nickname == after.Nickname)
            {
                return; 
            }

            if (before.Nickname is null)
            {
                return; 
            }
            
            var user = await _uchuumaruContext
                .Users
                .Where(x => x.Guild.GuildId == before.Guild.Id)
                .Where(x => x.UserId == before.Id)
                .Include(x => x.Nicknames)
                .FirstOrDefaultAsync();

            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == before.Guild.Id)
                .FirstOrDefaultAsync();

            _ = guild ?? throw new EntityNotFoundException<Guild>();
            
            if (user is null)
            {
                user = new User
                {
                    UserId = before.Id,
                    Guild = guild
                };

                _uchuumaruContext.Add(user);
            }
            
            user.Nicknames.Add(new Nickname { Value = before.Nickname });
            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}