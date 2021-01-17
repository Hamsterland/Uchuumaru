using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.Message
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information received from
    /// the MessageReceived event.
    /// </summary>
    public class MessageReceivedNotification : INotification
    {
        /// <summary>
        /// The received message.
        /// </summary>
        public SocketMessage Message { get; }

        /// <summary>
        /// Constructs a new <see cref="MessageReceivedNotification"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/></param>
        public MessageReceivedNotification(SocketMessage message)
        {
            Message = message;
        }
    }
}