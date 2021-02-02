using System.Threading.Tasks;
using Discord.WebSocket;
using Uchuumaru.Notifications.Guilds;
using Uchuumaru.Services.Guilds;

namespace Uchuumaru.Services
{
    public partial class DiscordHostedService
    {
        public async Task JoinedGuild(SocketGuild guild)
        {
            await _mediator.Publish(new JoinedGuildNotification(guild));
        }

        public async Task LeftGuild(SocketGuild guild)
        {
            await _mediator.Publish(new LeftGuildNotification(guild));
        }
    }
}