using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;
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

            if (user.Id == 330746772378877954)
            {
                var uchuu = new EmbedBuilder()
                    .WithColor(new Color(92, 132, 255))
                    .WithImageUrl("https://anilist.co/img/icons/android-chrome-512x512.png")
                    .WithTitle("[Uchuu's Profile](https://anilist.co/user/Uchuu/)")
                    .Build();

                await ReplyAsync(embed: uchuu);
                return;
            }
            
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
            var token = _verification.GetToken();
            var avatar = Context.User.GetAvatarUrl();
            
            var embed = new EmbedBuilder()
                .WithColor(Constants.DefaultColour)
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(avatar))
                .WithDescription($"{_loading} Please set your MyAnimeList account Location field to the Token below.")
                .AddField("Token", token, true)
                .AddField("Edit Profile", "https://myanimelist.net/editprofile.php", true)
                .WithFooter($"You have {VerificationService.RetryWaitPeriod * VerificationService.MaxRetries / 1000} seconds")
                .Build();

            var message = await ReplyAsync(embed: embed);
            var result = await _verification.Verify(Context.User as IGuildUser, username, token);

            if (result.IsSuccess)
            {
                await message.ModifyAsync(msg =>
                {
                    msg.Embed = new EmbedBuilder()
                        .WithColor(Color.Green)
                        .WithAuthor(author => author
                            .WithName($"{Context.User}")
                            .WithIconUrl(avatar))
                        .WithDescription("Successfully linked your account.")
                        .Build();
                });

                return;
            }

            await message.ModifyAsync(msg =>
            {
                msg.Embed = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithAuthor(a => a
                        .WithName($"{Context.User}")
                        .WithIconUrl(avatar))
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
        
        
    }
}