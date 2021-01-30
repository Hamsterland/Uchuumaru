using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Users
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information received from
    /// the UserJoined event.
    /// </summary>
    public class UserJoinedNotification : INotification
    {
        /// <summary>
        /// The user that joined.
        /// </summary>
        public SocketGuildUser User { get; }

        /// <summary>
        /// Constructs a new <see cref="UserJoinedNotification"/>.
        /// </summary>
        /// <param name="user">The user that joined.</param>
        public UserJoinedNotification(SocketGuildUser user)
        {
            User = user;
        }
    }
}