using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.Infractions.Mutes;

namespace Uchuumaru.Modules
{
    [Name("Mute")]
    [Summary("Silence, wench.")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public class MuteModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMuteService _mute;

        public MuteModule(IMuteService mute)
        {
            _mute = mute;
        }

        private readonly Random _random = new();
        
        private readonly string[] _muteMessages =
        {
            "Silenced {0} for {1}.",
            "Gagged {0} for {1}.",
            "{0} had their mouth sown shut for {1}.",
            "No one will have to listen to {0}'s opinions for {1}.",
            "{0} Larynx surgery will last for {1}."
        };

        private readonly string[] _unmuteMessages =
        {
            "{0} learned how to speak.",
            "{0} regained their participation privileges.",
            "{0} can now talk! Hooray!"
        };
        
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
            await ReplyMuteMessage(user, duration);
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
            
            await _mute.MuteCallback(Context.Guild.Id, Context.User.Id, mute.Id);
            await ReplyUnmuteMessage(user);
        }
        
        private async Task ReplyMuteMessage(IUser user, TimeSpan duration)
        {
            var message = _muteMessages[_random.Next(0, _muteMessages.Length - 1)];
            await ReplyAsync(string.Format(message, user, duration));
        }
        
        private async Task ReplyUnmuteMessage(IUser user)
        {
            var message = _unmuteMessages[_random.Next(0, _unmuteMessages.Length - 1)];
            await ReplyAsync(string.Format(message, user));
        }
    }
}