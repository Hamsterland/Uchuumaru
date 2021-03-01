// using System;
// using System.Threading.Tasks;
// using Discord;
// using Discord.Commands;
// using Uchuumaru.Preconditions;
// using Uchuumaru.Services.MAL;
//
// namespace Uchuumaru.Modules
// {
//     [Name("MyAnimeList")]
//     [Group("mal")]
//     [Summary("I assert that Anilist is better.")]
//     public class MALModule : ModuleBase<SocketCommandContext>
//     {
//         private readonly IProfileParser _parser;
//         private readonly IVerificationService _verification;
//
//         public MALModule(
//             IProfileParser parser,
//             IVerificationService verification)
//         {
//             _parser = parser;
//             _verification = verification;
//         }
//
//         [Command]
//         [Summary("Finds a user's MAL profile.")]
//         [RequireGuild(225938919756136448)]
//         public async Task MAL(IGuildUser user = null)
//         {
//             user ??= Context.User as IGuildUser;
//             var profile = await _verification.GetProfile(user.Id);
//             var embed = GetProfileEmbed(profile);
//             await ReplyAsync(embed: embed);
//         }
//
//         [Command("search")]
//         [Summary("Searches for a MAL user.")]
//         public async Task Search(string username)
//         {
//             var profile = await _parser.Parse(username);
//             var embed = GetProfileEmbed(profile);
//             await ReplyAsync(embed: embed);
//         }
//
//         [Command("code")]
//         [Summary("Gets or creates your Verification Code.")]
//         [RequireGuild(225938919756136448)]
//         public async Task Code()
//         {
//             await _verification.Start(Context.Guild.Id, Context.User.Id);
//         }
//
//         [Command("set")]
//         [Summary("Links your MAL to your Discord account.")]
//         [RequireGuild(225938919756136448)]
//         public async Task Verify(string username)
//         {
//             await _verification.Confirm(Context.User.Id, username);
//             await ReplyAsync("Linked your account!");
//         }
//
//         private static Embed GetProfileEmbed(Profile profile)
//         {
//             var builder = new EmbedBuilder();
//
//             builder
//                 .WithTitle($"{Format.Sanitize(profile.Name)}'s Profile")
//                 .WithUrl(profile.ProfileUrl)
//                 .WithDescription($"[Anime List]({profile.AnimeListUrl}) • [Manga List]({profile.MangaListUrl})")
//                 .WithThumbnailUrl(profile.ImageUrl)
//                 .AddField(":alarm_clock: Last Online", profile.RawLastOnline, true);
//             
//             return builder
//                 .AddField($"{profile.GenderEmote} Gender", profile.FormattedGender, true)
//                 .AddField(":date: Birthday", profile.RawBirthday, true)
//                 .AddField(":map: Location", profile.Location, true)
//                 .AddField(":hourglass: Joined", profile.DateJoined.ToString("MMM d, yyyy"), true)
//                 .AddField(":bar_chart: Mean Score", profile.MeanScore, true)
//                 .WithColor(Constants.DefaultColour)
//                 .Build();
//         }
//     }
// }