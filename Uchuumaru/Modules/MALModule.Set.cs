using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Preconditions;
using Uchuumaru.Services.MyAnimeList;

namespace Uchuumaru.Modules
{
    public partial class MALModule
    {
        private readonly Emote _loading = Emote.Parse("<a:loading:818260297118384178>");

        [Command("set", RunMode = RunMode.Async)]
        [Summary("Sets a MAL account.")]
        [MyAnimeListOnly]
        public async Task Set(string username)
        {
            var user = Context.User as IGuildUser;
            var avatar = Context.User.GetAvatarUrl();
            
            var embed = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(avatar));

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
                .WithFooter(
                    $"You have {VerificationService.RetryWaitPeriod * VerificationService.MaxRetries / 1000} seconds")
                // .WithImageUrl("https://timer.plus/" +
                //               $"{DateTime.Now.Year:####}," +
                //               $"{DateTime.Now.Month:##}," +
                //               $"{DateTime.Now.Day:##}," +
                //               $"{DateTime.Now.Hour - 5:##}," +
                //               $"{DateTime.Now.Minute + 1:##}," +
                //               $"{DateTime.Now.Second:##}.gif")
                .Build();

            var message = await ReplyAsync(embed: embed.Build());

            result = await _verification.Verify(user, username, token);
            if (result.IsSuccess)
            {
                await message.ModifyAsync(msg =>
                {
                    msg.Embed = new EmbedBuilder()
                        .WithAuthor(author => author
                            .WithName($"{Context.User}")
                            .WithIconUrl(avatar))
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
                        .WithAuthor(author => author
                            .WithName($"{Context.User}")
                            .WithIconUrl(avatar))
                    .WithColor(Color.Red)
                    .WithDescription(result.ErrorReason)
                    .Build();
            });
        }
    }
}