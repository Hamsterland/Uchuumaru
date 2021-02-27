using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.MAL;

namespace Uchuumaru.Modules
{
    public class MALModule : ModuleBase<SocketCommandContext>
    {
        private readonly ProfileParser _parser;

        public MALModule(ProfileParser parser)
        {
            _parser = parser;
        }

        [Command("malsearch")]
        [Summary("Searches for a MAL user.")]
        public async Task Search(string username)
        {
            var profile = await _parser.ParseProfile(username);
            
            var embed = new EmbedBuilder()
                .WithTitle($"{profile.Name}'s Profile")
                .WithUrl(profile.ProfileUrl)
                .WithDescription($"[Anime List]({profile.AnimeListUrl}) ~ [Manga List]({profile.MangaListUrl})")
                .WithThumbnailUrl(profile.ImageUrl)
                .AddField("Location", profile.Location, true)
                .AddField("Joined", profile.DateJoined.ToString("MMM d, yyyy"), true)
                .AddField("Birthday", profile.Birthday, true)
                .AddField("Last Online", profile.LastOnline, true)
                .AddField("Gender", profile.Gender, true)
                .AddField("Score", profile.Score, true)
                .WithColor(Constants.DefaultColour)
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}