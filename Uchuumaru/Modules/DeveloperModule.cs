using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("Developer")]
    [Summary("Commands restricted to developers.")]
    [RequireDeveloper]
    public class DeveloperModule : ModuleBase<SocketCommandContext>
    {
        private readonly IHostApplicationLifetime _lifetime;

        public DeveloperModule(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        [Command("echo")]
        [Summary("Echoes a message.")]
        public async Task Echo([Remainder] string message)
        {
            await ReplyAsync(message);
        }

        [Command("restart")]
        [Summary("Restarts the bot.")]
        public Task Restart()
        {
            Process.Start("Uchuumaru.exe");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}