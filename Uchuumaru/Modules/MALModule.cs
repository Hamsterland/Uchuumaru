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