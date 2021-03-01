using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;
using Uchuumaru.Notifications.Message;

namespace Uchuumaru.Services.Users
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> for the
    /// <see cref="MessageReceivedNotification"/>.
    /// </summary>
    public class UserListener : INotificationHandler<MessageReceivedNotification>
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="UserListener"/> with the
        /// given injected dependencies.
        /// </summary>
        public UserListener(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <summary>
        /// If the author of the received message is not in the database, then
        /// they are added.
        /// </summary>
        /// <exception cref="EntityNotFoundException{TEntity}">
        /// Thrown if the Guild with the same ID as the guild from the message
        /// is not found in the database.
        /// </exception>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.Message.Channel is IDMChannel)
                return;
            
            var author = notification.Message.Author;
            var guildId = (notification.Message.Channel as IGuildChannel).GuildId;

            var user = await _uchuumaruContext
                .Users
                .Where(x => x.Guild.GuildId == guildId)
                .FirstOrDefaultAsync(x => x.UserId == author.Id, cancellationToken);

            if (user is not null)
                return;

            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync(cancellationToken);

            _ = guild ?? throw new EntityNotFoundException<Guild>();

            _uchuumaruContext.Add(new User
            {
                UserId = author.Id,
                Guild = guild
            });

            await _uchuumaruContext.SaveChangesAsync(cancellationToken);
        }
    }
}