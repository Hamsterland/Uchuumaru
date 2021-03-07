using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("Developer")]
    [Summary("Commands restricted to developers.")]
    [RequireDeveloper]
    public class DeveloperModule : ModuleBase<SocketCommandContext>
    {
        [Command("pin")]
        [Summary("Pins a message.")]
        public async Task Ping(ulong messageId)
        {
            var message = await Context.Channel.GetMessageAsync(messageId) as IUserMessage;
            await message.PinAsync();
        }
        
        [Obsolete]
        [Command("restart")]
        [Summary("Restarts the bot.")]
        public Task Restart()
        {
            Process.Start("Uchuumaru.exe");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
        
        [Obsolete]
        [Command("shutdown")]
        [Summary("Shuts down the bot.")]
        public Task Shutdown()
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}