using System.Threading.Tasks;
using Discord.WebSocket;
using Uchuumaru.Notifications;

namespace Uchuumaru.Services
{
    public partial class DiscordListener
    {
        public async Task MessageReceived(SocketMessage message)
        {
            await _mediator.Publish(new MessageReceivedNotification(message));
        }
    }
}