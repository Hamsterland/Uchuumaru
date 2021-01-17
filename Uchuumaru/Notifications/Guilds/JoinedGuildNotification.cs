using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Guilds
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information sent from the
    /// JoinedGuild event.
    /// </summary>
    public class JoinedGuildNotification : INotification
    {
        /// <summary>
        /// The guild that was joined.
        /// </summary>
        public SocketGuild Guild { get; }

        /// <summary>
        /// Constructs a new <see cref="JoinedGuildNotification"/>.
        /// </summary>
        /// <param name="guild"><see cref="Guild"/></param>
        public JoinedGuildNotification(SocketGuild guild)
        {
            Guild = guild;
        }
    }
}