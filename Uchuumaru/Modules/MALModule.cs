// using System.Threading.Tasks;
// using Discord;
// using Discord.Commands;
// using Uchuumaru.MyAnimeList.Models;
// using Uchuumaru.MyAnimeList.Parsers;
// using Uchuumaru.Preconditions;
// using Uchuumaru.Services.MyAnimeList;
//
// namespace Uchuumaru.Modules
// {
//     [Name("MyAnimeList")]
//     [Group("mal")]
//     [Summary("I assert that Anilist is better.")]
//     [RequireDeveloper]
//     public class MALModule : ModuleBase<SocketCommandContext>
//     {
//         private readonly ProfileParser _parser;
//         private readonly IVerificationService _verification;
//
//         public MALModule(ProfileParser parser, IVerificationService verification)
//         {
//             _parser = parser;
//             _verification = verification;
//         }
//
//         // [Command]
//         // [Summary("Finds a user's MAL profile.")]
//         // [RequireGuild(225938919756136448)]
//         // public async Task MAL(IGuildUser user = null)
//         // {
//         //     user ??= Context.User as IGuildUser;
//         //     var profile = await _verification.GetProfile(user.Id);
//         //     var embed = GetProfileEmbed(profile);
//         //     await ReplyAsync(embed: embed);
//         // }
//
//         [Command]
//         [Summary("Finds a user's MAL account.")]
//         public async Task Search(IGuildUser user = null)
//         {
//             user ??= Context.User as IGuildUser;
//
//             var profile = await _verification.GetProfile(user.Id);
//
//             if (profile is null)
//             {
//                 await ReplyAsync($"{user} does not have an account linked.");
//                 return;
//             }
//
//             var embed = new ProfileEmbedBuilder(profile)
//                 .BuildFullEmbed();
//
//             await ReplyAsync(embed: embed);
//         }
//
//         [Command("set", RunMode = RunMode.Async)]
//         [Summary("Sets a MAL account.")]
//         public async Task Set(string username)
//         {
//             var token = _verification.GetToken();
//             
//             var embed = new EmbedBuilder()
//                 .WithColor(Constants.DefaultColour)
//                 .WithAuthor(author => author
//                     .WithName($"{Context.User}")
//                     .WithIconUrl(Context.User.GetAvatarUrl()))
//                 .WithDescription("Please set your MyAnimeList account Location field to the Token below.")
//                 .AddField("Token", token, true)
//                 .AddField("Edit Profile", "https://myanimelist.net/editprofile.php", true)
//                 .WithFooter("Your location is automatically checked every 10 seconds.")
//                 .Build();
//             
//             var message = await ReplyAsync(embed: embed);
//             var result = await _verification.Begin(Context.User.Id, username, token);
//             
//             
//             if (result.IsSuccess)
//             {
//                 await message.ModifyAsync(msg =>
//                 {
//                     msg.Embed = new EmbedBuilder()
//                         .WithColor(Color.Green)
//                         .WithTitle("Done!")
//                         .WithDescription($"{Context.User}, your account has been linked. See `+mal` to view your account information.")
//                         .Build();
//                 });
//
//                 return;
//             }
//
//             await ReplyAsync(result.ErrorReason);
//         }
//         
//         [Command("search")]
//         [Summary("Searches for a MAL user.")]
//         public async Task Search(string username)
//         {
//             var profile = await Profile.FromUsername(username, _parser);
//
//             var embed = new ProfileEmbedBuilder(profile)
//                 .BuildFullEmbed();
//
//             await ReplyAsync(embed: embed);
//         }
//
//         // [Command("code")]
//         // [Summary("Gets or creates your Verification Code.")]
//         // [RequireGuild(225938919756136448)]
//         // public async Task Code()
//         // {
//         //     await _verification.Start(Context.Guild.Id, Context.User.Id);
//         // }
//         //
//         // [Command("set")]
//         // [Summary("Links your MAL to your Discord account.")]
//         // [RequireGuild(225938919756136448)]
//         // public async Task Verify(string username)
//         // {
//         //     await _verification.Confirm(Context.User.Id, username);
//         //     await ReplyAsync("Linked your account!");
//         // }
//         
//     }
// }