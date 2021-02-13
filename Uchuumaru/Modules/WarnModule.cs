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
            var log = true;
            var myAnimeListOnly = new MyAnimeListOnly();
            var preconditionResult = await myAnimeListOnly.CheckPermissionsAsync(Context, null, null);

            // We don't want to log warns in MyAnimeList because Gil said so. 
            if (preconditionResult.IsSuccess)
                log = false;

            await _warn.Create(Context.Guild.Id, user.Id, Context.User.Id, log, reason);
            await ReplyAsync($"Warned {user}.");
        }

        [Command("rescind")]
        [Alias("revoke")]
        [Summary("Rescinds a warning.")]
        public async Task Rescind(int id)
        {
            await _warn.Rescind(id, Context.Guild.Id);
            await ReplyAsync($"Rescinded warning \"{id}\"."); 
        }
        
        [Command("delete")]
        [Summary("Rescinds a warning.")]
        public async Task Delete(int id)
        {
            await _warn.Delete(id, Context.Guild.Id);
            await ReplyAsync($"Deleted warning \"{id}\"."); 
        }
    }
}