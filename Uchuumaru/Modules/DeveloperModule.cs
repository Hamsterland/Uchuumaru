using System.Threading.Tasks;
using Discord.Commands;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("Developer")]
    [Summary("Commands restricted to developers.")]
    [RequireDeveloper]
    public class DeveloperModule : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        [Summary("Echoes a message.")]
        public async Task Echo([Remainder] string message)
        {
            await ReplyAsync(message);
        }
    }
}