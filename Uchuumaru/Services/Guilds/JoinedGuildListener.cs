using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uchuumaru.Notifications.Guilds;

namespace Uchuumaru.Services.Guilds
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> for the <see cref="JoinedGuildNotification"/>.
    /// </summary>
    public class JoinedGuildListener : INotificationHandler<JoinedGuildNotification>
    {
        /// <summary>
        /// The guild service.
        /// </summary>
        private readonly IGuildService _guild;

        /// <summary>
        /// Constructs a new <see cref="JoinedGuildListener"/> with the
        /// given injected dependencies.
        /// </summary>
        public JoinedGuildListener(IGuildService guild)
        {
            _guild = guild;
        }

        /// <summary>
        /// Adds the guild that was just joined to the database.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(JoinedGuildNotification notification, CancellationToken cancellationToken)
        {
            await _guild.AddGuild(notification.Guild.Id);
        }
    }
}