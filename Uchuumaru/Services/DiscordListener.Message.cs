using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Uchuumaru.Notifications;
using Uchuumaru.Notifications.Message;

namespace Uchuumaru.Services
{
    public partial class DiscordListener
    {
        public async Task MessageReceived(SocketMessage message)
        {
            await _mediator.Publish(new MessageReceivedNotification(message));
        }
        
        private async Task MessageDeleted(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            await _mediator.Publish(new MessageDeletedNotification(message, channel));
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            await _mediator.Publish(new MessageUpdatedNotification(before, after, channel));
        }
    }
}