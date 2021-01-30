using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Users
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information received from
    /// the UserLeft event.
    /// </summary>
    public class UserLeftNotification : INotification
    {
        /// <summary>
        /// The user that left.
        /// </summary>
        public SocketGuildUser User { get; }

        /// <summary>
        /// Constructs a new <see cref="UserJoinedNotification"/>.
        /// </summary>
        /// <param name="user">The user that left.</param>
        public UserLeftNotification(SocketGuildUser user)
        {
            User = user;
        }
    }
}