using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Notifications.Users;

namespace Uchuumaru.Services.Users
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the
    /// <see cref="UserUpdatedNotification"/>.
    /// </summary>
    public class UserUpdatedListener : INotificationHandler<UserUpdatedNotification>
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="UserUpdatedListener"/> with
        /// the given inject dependencies.
        /// </summary>
        public UserUpdatedListener(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <summary>
        /// Initiates the sequence of checks to be carried out on this
        /// event.
        /// 1) Determines if the username has changed.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(UserUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var before = notification.Before;
            var after = notification.After;
            await CompareUsernames(before, after);
        }

        /// <summary>
        /// Determines if the username has changed.
        /// </summary>
        /// <param name="before">The user before update.</param>
        /// <param name="after">The user after update.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task CompareUsernames(SocketUser before, SocketUser after)
        {
            if (before.Username == after.Username)
            {
                return; 
            }

            var users = _uchuumaruContext
                .Users
                .Where(x => x.UserId == before.Id)
                .Include(x => x.Usernames);

            foreach (var user in users)
            {
                user.Usernames.Add(new Username { Value = before.Username });
            }

            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}