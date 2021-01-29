using Discord;
using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information received from the
    /// MessageDeleted event.
    /// </summary>
    public class MessageDeletedNotification : INotification
    {
        /// <summary>
        /// The deleted message.
        /// </summary>
        public Cacheable<IMessage, ulong> Message { get; }
        
        /// <summary>
        /// The channel where the message was deleted.
        /// </summary>
        public ISocketMessageChannel Channel { get; }

        /// <summary>
        /// Constructs a new <see cref="MessageDeletedNotification"/>.
        /// </summary>
        /// <param name="message">The deleted message.</param>
        /// <param name="channel">The channel where the message was deleted.</param>
        public MessageDeletedNotification(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            Message = message;
            Channel = channel;
        }
    }
}