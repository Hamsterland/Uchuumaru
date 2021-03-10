using System;
using System.Linq;
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
        
        private readonly Emote _loading = Emote.Parse("<a:loading:818260297118384178>");
        
        [Command]
        [Summary("Finds a user's MAL account.")]
        public async Task Search(IGuildUser user = null)
        {
            user ??= Context.User as IGuildUser;

            Profile profile;
            
            switch (user.Id)
            {
                // Uchuu
                case 330746772378877954:
                {
                    profile = await _verification.GetProfile(330746772378877954);
                    await ReplyAsync(embed: UchuuProfileEmbed(profile));
                    return;
                }
                
                // Tincan 
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
        
        
        [Command("set", RunMode = RunMode.Async)]
        [Summary("Sets a MAL account.")]
        [MyAnimeListOnly]
        public async Task Set(string username)
        {
            var user = Context.User as IGuildUser;

            var embed = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(Context.User.GetAvatarUrl()));

            if (!await _verification.AccountExists(username))
            {
                embed.WithColor(Color.Red)
                    .WithDescription($"Failed to verify. Account \"{username}\" does not exist.");
                await ReplyAsync(embed: embed.Build());
                return;
            }
            
            VerificationResult result;
            
            var roles = user
                .RoleIds
                .ToList();


            IRole verified = null;
            if (Context.Guild.Id == 301123999000166400)
            {
                verified = Context.Guild.GetRole(372178027926519810);
                
                if (!roles.Contains(verified.Id))
                {
                    result = await _verification.Authenticate(username);

                    if (!result.IsSuccess)
                    {
                        embed.WithColor(Color.Red)
                            .WithDescription(result.ErrorReason);
                        await ReplyAsync(embed: embed.Build());
                        return;
                    }               
                }        
            }
            
            var token = _verification.GetToken();

            embed
                .WithColor(Constants.DefaultColour)
                .WithDescription($"{_loading} Please set your MyAnimeList account Location field to the Token below.")
                .AddField("Token", token, true)
                .AddField("Edit Profile", "https://myanimelist.net/editprofile.php", true)
                .WithFooter($"You have {VerificationService.RetryWaitPeriod * VerificationService.MaxRetries / 1000} seconds")
                .Build();

            var message = await ReplyAsync(embed: embed.Build());
            
            result = await _verification.Verify(user, username, token);

            if (result.IsSuccess)
            {
                await message.ModifyAsync(msg =>
                {
                    msg.Embed = new EmbedBuilder()
                        .WithColor(Color.Green)
                        .WithDescription("Successfully linked your account.")
                        .Build();
                });

                if (Context.Guild.Id == 301123999000166400)
                    await user.AddRoleAsync(verified);
                return;
            }

            await message.ModifyAsync(msg =>
            {
                msg.Embed = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithDescription(result.ErrorReason)
                    .Build();
            });
        }
        
        [Command("search")]
        [Summary("Searches for a MAL user.")]
        public async Task Search(string username)
        {
            var profile = await Profile.FromUsername(username, _parser);
            var embed = new ProfileEmbedBuilder(profile).BuildFullEmbed();
            await ReplyAsync(embed: embed);
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