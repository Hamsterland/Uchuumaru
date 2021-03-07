using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;
using Uchuumaru.Preconditions;
using Uchuumaru.Services.MyAnimeList;

namespace Uchuumaru.Modules
{
    [Name("MyAnimeList")]
    [Group("mal")]
    [Summary("I assert that Anilist is better.")]
    [RequireRoles(
        301125242749714442,     // Moderator
        389930012855238657,     // Retired Moderator
        302310311074070531,     // Site Admin
        301127756467535885,     // Site Staff
        312948066661695488,     // Retired Site Staff
        674424406600319018,     // Former Admin
        781950226423349298,     // Rewrite Moderator
        225940071516209153,     // Coordinator
        460285613040730112)]    // Botcon
    public class MALModule : ModuleBase<SocketCommandContext>
    {
        private readonly ProfileParser _parser;
        private readonly IVerificationService _verification;

        public MALModule(ProfileParser parser, IVerificationService verification)
        {
            _parser = parser;
            _verification = verification;
        }
        
        [Command]
        [Summary("Finds a user's MAL account.")]
        public async Task Search(IGuildUser user = null)
        {
            user ??= Context.User as IGuildUser;

            var profile = await _verification.GetProfile(user.Id);

            if (profile is null)
            {
                await ReplyAsync($"{user} does not have an account linked.");
                return;
            }

            var embed = new ProfileEmbedBuilder(profile).BuildFullEmbed();
            await ReplyAsync(embed: embed);
        }
        
        
        [Command("set", RunMode = RunMode.Async)]
        [Summary("Sets a MAL account.")]
        public async Task Set(string username)
        {
            await _verification.Begin(Context.User as IGuildUser, username, Context.Channel as ITextChannel);
        }
        
        [Command("search")]
        [Summary("Searches for a MAL user.")]
        public async Task Search(string username)
        {
            var profile = await Profile.FromUsername(username, _parser);
            var embed = new ProfileEmbedBuilder(profile).BuildFullEmbed();
            await ReplyAsync(embed: embed);
        }
        
        
    }
}