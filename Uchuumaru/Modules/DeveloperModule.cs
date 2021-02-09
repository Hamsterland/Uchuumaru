using System;
using System.Diagnostics;
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
        [Command("restart")]
        [Summary("Restarts the bot.")]
        public Task Restart()
        {
            Process.Start("Uchuumaru.exe");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
        
        [Command("shutdown")]
        [Summary("Shuts down the bot.")]
        public Task Shutdown()
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}