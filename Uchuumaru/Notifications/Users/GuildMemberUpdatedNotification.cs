using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Users
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information
    /// from when a user in a guild is updated.
    /// </summary>
    public class GuildMemberUpdatedNotification : INotification
    {
        /// <summary>
        /// The user before update.
        /// </summary>
        public SocketGuildUser Before { get; }
        
        /// <summary>
        /// The user after update.
        /// </summary>
        public SocketGuildUser After { get; }

        /// <summary>
        /// Constructs a new <see cref="GuildMemberUpdatedNotification"/>.
        /// </summary>
        /// <param name="before">The user before update.</param>
        /// <param name="after">The user after update.</param>
        public GuildMemberUpdatedNotification(SocketGuildUser before, SocketGuildUser after)
        {
            Before = before;
            After = after;
        }
    }
}