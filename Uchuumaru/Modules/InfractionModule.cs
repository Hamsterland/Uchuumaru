using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Data.Models;
using Uchuumaru.Services.Infractions;

namespace Uchuumaru.Modules
{
    [Name("Infractions")]
    [Group("infraction")]
    [Summary("Santa's little helper.")]
    public class InfractionModule : ModuleBase<SocketCommandContext>
    {
        private readonly IInfractionService _infraction;

        public InfractionModule(IInfractionService infraction)
        {
            _infraction = infraction;
        }
        
        [Command("claim")]
        [Summary("Claims an infraction.")]
        public async Task Claim(ulong messageId)
        {
            await _infraction.ClaimInfraction(Context.Guild.Id, messageId, Context.User.Id);
            await ReplyAsync($"Claimed this infraction for {Context.User} ({Context.User.Id})");
        }

        [Command("claim")]
        [Summary("Claims an infraction with a reason.")]
        public async Task Claim(ulong messageId, [Remainder] string reason)
        {
            await _infraction.ClaimInfraction(Context.Guild.Id, messageId, Context.User.Id, reason);
            await ReplyAsync($"Claimed this infraction for {Context.User} ({Context.User.Id}) with reason \"{reason}\".");
        }
        
        [Group("channel")]
        [Summary("Where will the infractions log?")]
        public class ChannelModule : ModuleBase<SocketCommandContext>
        {
            private readonly IInfractionService _infraction;

            public ChannelModule(IInfractionService infraction)
            {
                _infraction = infraction;
            }

            [Command("set")]
            [Summary("Sets the filter channel.")]
            public async Task Set(IGuildChannel channel)
            {
                await _infraction.ModifyInfractionChannel(ChannelModificationOptions.Set, Context.Guild.Id, channel.Id);
                await ReplyAsync($"Set the infraction channel to <#{channel.Id}> ({channel.Id})");
            }
            
            [Command("remove")]
            [Summary("Removes the filter channel.")]
            public async Task Remove()
            {
                await _infraction.ModifyInfractionChannel(ChannelModificationOptions.Remove, Context.Guild.Id);
                await ReplyAsync("Removed the infraction channel.");
            }
        }
    }
}