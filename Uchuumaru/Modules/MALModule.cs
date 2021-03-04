using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("MyAnimeList")]
    [Group("mal")]
    [Summary("I assert that Anilist is better.")]
    [RequireDeveloper]
    public class MALModule : ModuleBase<SocketCommandContext>
    {
        private readonly ProfileParser _parser;

        public MALModule(ProfileParser parser)
        {
            _parser = parser;
        }

        // [Command]
        // [Summary("Finds a user's MAL profile.")]
        // [RequireGuild(225938919756136448)]
        // public async Task MAL(IGuildUser user = null)
        // {
        //     user ??= Context.User as IGuildUser;
        //     var profile = await _verification.GetProfile(user.Id);
        //     var embed = GetProfileEmbed(profile);
        //     await ReplyAsync(embed: embed);
        // }

        [Command("search")]
        [Summary("Searches for a MAL user.")]
        public async Task Search(string username)
        {
            var profile = await Profile.FromUsername(username, _parser);
            await ReplyAsync(embed: GetProfileEmbed(profile));
        }

        // [Command("code")]
        // [Summary("Gets or creates your Verification Code.")]
        // [RequireGuild(225938919756136448)]
        // public async Task Code()
        // {
        //     await _verification.Start(Context.Guild.Id, Context.User.Id);
        // }
        //
        // [Command("set")]
        // [Summary("Links your MAL to your Discord account.")]
        // [RequireGuild(225938919756136448)]
        // public async Task Verify(string username)
        // {
        //     await _verification.Confirm(Context.User.Id, username);
        //     await ReplyAsync("Linked your account!");
        // }

        private static Embed GetProfileEmbed(Profile profile)
        {
            var builder = new EmbedBuilder();

            builder
                .WithTitle($"{Format.Sanitize(profile.Username)}'s Profile")
                .WithUrl(profile.Url)
                .WithDescription($"[Anime List]({profile.AnimeList.Url}) • [Manga List]({profile.MangaList.Url})")
                .WithThumbnailUrl(profile.ImageUrl)
                .AddField(":alarm_clock: Last Online", profile.LastOnline, true);
            
            return builder
                .AddField($"Gender", profile.Gender, true)
                .AddField(":date: Birthday", profile.Birthday, true)
                .AddField(":map: Location", profile.Location, true)
                .AddField(":hourglass: Joined", profile.DateJoined.ToString("MMM d, yyyy"), true)
                .AddField(":bar_chart: Mean Score", profile.AnimeList.MeanScore, true)
                .WithColor(Constants.DefaultColour)
                .WithImageUrl(profile.ImageUrl)
                .Build();
        }
    }
}