using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.MyAnimeList;
using static Uchuumaru.Constants;

namespace Uchuumaru.Modules
{
    public partial class MALModule
    {
        private readonly Emote _loading = Emote.Parse("<a:loading:818260297118384178>");

        [Command("set", RunMode = RunMode.Async)]
        [Summary("Sets a MAL account.")]
        public async Task Set(string username)
        {
            var user = Context.User as IGuildUser;
            var avatar = Context.User.GetAvatarUrl();

            var builder = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(avatar));

            if (!await _verification.AccountExists(username))
            {
                AppendNonExistantAccountToEmbed(builder, username);
                await ReplyAsync(embed: builder.Build());
                return;
            }

            VerificationResult result;

            var roles = user
                .RoleIds
                .ToList();

            IRole verified = null;
            if (Context.Guild.Id == MyAnimeListId)
            {
                verified = Context.Guild.GetRole(VerifiedId);

                if (!roles.Contains(verified.Id))
                {
                    result = await _verification.Authenticate(username);

                    if (!result.IsSuccess)
                    {
                        builder
                            .WithColor(Color.Red)
                            .WithDescription(result.ErrorReason);

                        await ReplyAsync(embed: builder.Build());
                        return;
                    }
                }
            }
            
            var token = _verification.GetToken();
            AppendVerifyAccountToEmbed(builder, token.ToString());
            var message = await ReplyAsync(embed: builder.Build());
            
            result = await _verification.Verify(user, username, token);
            if (result.IsSuccess)
            {
                await message.ModifyAsync(msg => msg.Embed = GetSuccessEmbed(avatar));

                if (Context.Guild.Id == MyAnimeListId)
                    await user.AddRoleAsync(verified);
                
                return;
            }

            await message.ModifyAsync(msg => msg.Embed = GetFailureEmbed(result, avatar));
        }

        private Embed GetSuccessEmbed(string avatar)
        {
            return new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(avatar))
                .WithColor(Color.Green)
                .WithDescription("Successfully linked your account.")
                .Build();
        }

        public Embed GetFailureEmbed(VerificationResult result, string avatar)
        {
            return new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{Context.User}")
                    .WithIconUrl(avatar))
                .WithColor(Color.Red)
                .WithDescription(result.ErrorReason)
                .Build();
        }

        private void AppendVerifyAccountToEmbed(EmbedBuilder builder, string token)
        {
            builder
                .WithColor(DefaultColour)
                .WithDescription($"{_loading} Please set your MyAnimeList account Location field to the Token below.")
                .AddField("Token", token, true)
                .AddField("Edit Profile", "https://myanimelist.net/editprofile.php", true)
                .WithFooter($"You have {VerificationService.RetryWaitPeriod * VerificationService.MaxRetries / 1000} seconds");
        }

        private static void AppendNonExistantAccountToEmbed(EmbedBuilder builder, string username)
        {
            builder
                .WithColor(Color.Red)
                .WithDescription($"Failed to verify. Account \"{username}\" does not exist.");
        }
    }
}