using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Users
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information
    /// from when a user is updated.
    /// </summary>
    public class UserUpdatedNotification : INotification
    {
        /// <summary>
        /// The user before update.
        /// </summary>
        public SocketUser Before { get; }
        
        /// <summary>
        /// The user after update.
        /// </summary>
        public SocketUser After { get; }

        /// <summary>
        /// Constructs a new <see cref="UserUpdatedNotification"/>.
        /// </summary>
        /// <param name="before"><see cref="Before"/></param>
        /// <param name="after"><see cref="After"/></param>
        public UserUpdatedNotification(SocketUser before, SocketUser after)
        {
            Before = before;
            After = after;
        }
    }
}