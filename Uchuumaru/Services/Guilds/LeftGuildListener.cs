using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uchuumaru.Notifications.Guilds;

namespace Uchuumaru.Services.Guilds
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> for the <see cref="LeftGuildNotification"/>.
    /// </summary>
    public class LeftGuildListener : INotificationHandler<LeftGuildNotification>
    {
        /// <summary>
        /// The guild service.
        /// </summary>
        private readonly IGuildService _guild;

        /// <summary>
        /// Constructs a new <see cref="LeftGuildListener"/> with the given
        /// injected dependencies.
        /// </summary>
        /// <param name="guild"></param>
        public LeftGuildListener(IGuildService guild)
        {
            _guild = guild;
        }

        /// <summary>
        /// Removes the guild that was just left from the database.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(LeftGuildNotification notification, CancellationToken cancellationToken)
        {
            await _guild.RemoveGuild(notification.Guild.Id);
        }
    }
}