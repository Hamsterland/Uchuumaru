using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Guilds
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information sent from the
    /// LeftGuild event.
    /// </summary>
    public class LeftGuildNotification : INotification
    {
        /// <summary>
        /// The guild that was left.
        /// </summary>
        public SocketGuild Guild { get; }

        /// <summary>
        /// Constructs a new <see cref="LeftGuildNotification"/>.
        /// </summary>
        /// <param name="guild"><see cref="Guild"/></param>
        public LeftGuildNotification(SocketGuild guild)
        {
            Guild = guild;
        }
    }
}