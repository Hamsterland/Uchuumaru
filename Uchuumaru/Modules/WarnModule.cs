using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Preconditions;
using Uchuumaru.Services.Infractions.Warns;

namespace Uchuumaru.Modules
{
    [Name("Warn")]
    [Group("warn")]
    [Summary("Too afraid to mute? :(")]
    [RequireModeratorOrDeveloper]
    public class WarnModule : ModuleBase<SocketCommandContext>
    {
        private readonly IWarnService _warn;

        public WarnModule(IWarnService warn)
        {
            _warn = warn;
        }

        [Command]
        [Summary("Warns a user.")]
        public async Task Warn(IGuildUser user, [Remainder] string reason = null)
        {
            await _warn.CreateWarn(Context.Guild.Id, user.Id, Context.User.Id, reason);
            await ReplyAsync($"Warned {user}.");
        }
    }
}