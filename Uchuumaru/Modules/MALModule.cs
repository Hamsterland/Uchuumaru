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

            var embed = new ProfileEmbedBuilder(profile)
                .WithName()
                .WithProfileImage()
                .WithListUrls()
                .WithLastOnline()
                .WithGender()
                .WithBirthday()
                .WithLocation()
                .WithDateJoined()
                .WithMeanScore()
                .Build();

            await ReplyAsync(embed: embed);
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
        
    }
}