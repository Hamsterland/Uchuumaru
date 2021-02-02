using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Notifications.Commands;

namespace Uchuumaru.Services
{
    public partial class DiscordHostedService
    {
        public async Task CommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            await _mediator.Publish(new CommandExecutedNotification(command, context, result));
        }
    }
}