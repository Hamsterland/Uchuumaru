using Discord;
using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information from the
    /// MessageUpdated event.
    /// </summary>
    public class MessageUpdatedNotification : INotification
    {
        /// <summary>
        /// The message before update.
        /// </summary>
        public Cacheable<IMessage, ulong> Before { get; }
        
        /// <summary>
        /// The message after update.
        /// </summary>
        public SocketMessage After { get; }
        
        /// <summary>
        /// The channel where the message was updated.
        /// </summary>
        public ISocketMessageChannel Channel { get; }
        
        /// <summary>
        /// Constructs a new <see cref="MessageUpdatedNotification"/>.
        /// </summary>
        /// <param name="before">The message before update.</param>
        /// <param name="after">The message after update.</param>
        /// <param name="channel">The channel where the message was updated.</param>
        public MessageUpdatedNotification(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            Before = before;
            After = after;
            Channel = channel;
        }
    }
}