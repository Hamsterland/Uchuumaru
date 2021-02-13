﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("Channel")]
    [Summary("For large-scale channel-related tasks.")]
    public class ChannelModule : ModuleBase<SocketCommandContext>
    {
        [Command("prune")]
        [Alias("clean")]
        [Summary("Prunes messages from the channel. The default is 100.")]
        public async Task Prune(int size = 100)
        {
            var messages = await Context.Channel.GetMessagesAsync(size).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
            await ReplyAsync($"Deleted {size} messages.");
        }

        [Command("lock")]
        [Summary("Locks a channel.")]
        [ReadyForUse(false)]
        // [RequireExecutionContext(ExecutionContext.DEVELOPMENT)]
        public async Task Lock()
        {
            var everyone = Context.Guild.EveryoneRole;
            await (Context.Channel as IGuildChannel).AddPermissionOverwriteAsync(everyone,
                new OverwritePermissions(sendMessages: PermValue.Deny));
        }
        
        [Command("unlock")]
        [Summary("Unlocks a channel.")]
        [ReadyForUse(false)]
        // [RequireExecutionContext(ExecutionContext.DEVELOPMENT)]
        public async Task Unlock()
        {
            var everyone = Context.Guild.EveryoneRole;
            await (Context.Channel as IGuildChannel).AddPermissionOverwriteAsync(everyone,
                new OverwritePermissions(sendMessages: PermValue.Allow));
        }
    }
}