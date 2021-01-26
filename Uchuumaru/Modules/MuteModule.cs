using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.Infractions.Mutes;

namespace Uchuumaru.Modules
{
    [Name("Mute")]
    [Summary("Silence, wench.")]
    public class MuteModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMuteService _mute;

        public MuteModule(IMuteService mute)
        {
            _mute = mute;
        }

        [Command("mute")]
        [Summary("Mutes a user.")]
        public async Task Mute(IGuildUser user, TimeSpan duration, string reason = null)
        {
            var active = await _mute.GetActiveMute(Context.Guild.Id, user.Id);
            
            if (active is not null)
            {
                await ReplyAsync($"{user} already has an active mute. Please unmute them first.");
                return; 
            }

            await _mute.CreateMute(Context.Guild.Id, user.Id, Context.User.Id, duration, reason);
            await ReplyAsync($"Muted {user} for {duration}.");
        }

        [Command("unmute")]
        [Summary("Unmutes a user.")]
        public async Task Unmute(IGuildUser user)
        {
            var mute = await _mute.GetActiveMute(Context.Guild.Id, user.Id);

            if (mute is null)
            {
                await ReplyAsync($"{user} does not have an active mute.");
                return; 
            }
            
            await _mute.MuteCallback(Context.Guild.Id, mute.Id);
            await ReplyAsync($"Unmuted {user}.");
        }
    }
}