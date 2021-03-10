using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;
using Uchuumaru.Services.MyAnimeList;
using Uchuumaru.TypeReaders;

namespace Uchuumaru.Modules
{
    [Name("MyAnimeList")]
    [Group("mal")]
    [Summary("I assert that Anilist is better.")]
    public partial class MALModule : ModuleBase<SocketCommandContext>
    {
        private readonly ProfileParser _parser;
        private readonly IVerificationService _verification;

        public MALModule(
            ProfileParser parser,
            IVerificationService verification)
        {
            _parser = parser;
            _verification = verification;
        }
        
        [Command]
        [Summary("Finds a user's MAL account.")]
        public async Task Search([Remainder] IGuildUser user = null)
        {
            user ??= Context.User as IGuildUser;
            
            Profile profile;
            
            switch (user.Id)
            {
                case 330746772378877954:
                {
                    profile = await _verification.GetProfile(330746772378877954);
                    await ReplyAsync(embed: UchuuProfileEmbed(profile));
                    return;
                }
                case 153286487314661376:
                    profile = await _verification.GetProfile(422805690818625567); // Yuna
                    break;
                default:
                    profile = await _verification.GetProfile(user.Id);
                    break;
            }

            if (profile is null)
            {
                await ReplyAsync($"{user} does not have an account linked.");
                return;
            }

            var embed = new ProfileEmbedBuilder(profile).BuildFullEmbed();
            await ReplyAsync(embed: embed);
        }
        
        
        [Command("search")]
        [Summary("Searches for a MAL user.")]
        public async Task Search(string username)
        {
            var profile = await Profile.FromUsername(username, _parser);
            var embed = new ProfileEmbedBuilder(profile).BuildFullEmbed();
            await ReplyAsync(embed: embed);
        }

        [Command("id")]
        [Summary("Gets a user's MyAnimeList Id.")]
        public async Task Id(string username)
        {
            if (!await _verification.AccountExists(username))
            {
                await ReplyAsync($"Account \"{username}\" does not exist.");
                return;
            }

            await _parser.Refresh(username);
            var id = _parser.GetUserId();
            await ReplyAsync($"{username}'s MyAnimeList Id is {id}.");
        }
        
        private static Embed UchuuProfileEmbed(Profile profile)
        {
            return new ProfileEmbedBuilder(profile)
                .WithLastOnline()
                .WithGender()
                .WithBirthday()
                .WithLocation()
                .WithDateJoined()
                .WithMeanScore()
                .WithColor(92, 132, 255)
                .WithThumbnailUrl("https://anilist.co/img/icons/android-chrome-512x512.png")
                .WithImageUrl("https://i.imgur.com/2eO7DCI.png")
                .WithTitle("Uchuu's Profile")
                .WithUrl("https://anilist.co/user/Uchuu/")
                .WithDescription("[Anime List](https://anilist.co/user/Uchuu/animelist) • [Manga List](https://anilist.co/user/Uchuu/mangalist)")
                .Build();
        }
    }
}