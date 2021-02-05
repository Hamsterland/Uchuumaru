using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Uchuumaru.Modules
{
    [Name("Help")]
    [Group("help")]
    [Summary("Ye old handbook.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;

        public HelpModule(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command]
        [Summary("HEELPPP MEEE.")]
        public async Task Help()
        {
            var builder = new EmbedBuilder()
                .WithColor(Constants.DefaultColour)
                .WithAuthor(author => author
                    .WithName(_client.CurrentUser.Username)
                    .WithIconUrl(_client.CurrentUser.GetAvatarUrl()));

            var description = new StringBuilder()
                .AppendLine($"{_client.CurrentUser.Username} is an intricate moderation bot. It encompasses infractions, filters, and user information inside an extensible command framework.")
                .AppendLine()
                .AppendLine(Format.Bold("Help"))
                .AppendLine("Please see the online [command reference](https://github.com/Hamsterland/Uchuumaru/wiki/Commands).")
                .AppendLine()
                .AppendLine(Format.Bold("Premium"))
                .AppendLine("Sike, there is no premium.")
                .AppendLine()
                .AppendLine(Format.Bold("Invite"))
                .AppendLine($"There is no invite either. See the [GitHub instructions](https://github.com/Hamsterland/Uchuumaru) on running your own instance of {_client.CurrentUser.Username}.")
                .AppendLine()
                .AppendLine(Format.Bold("Support"))
                .AppendLine("Seek answers from your God. Sorry, atheists.")
                .AppendLine()
                .AppendLine(Format.Bold("Credit"))
                .AppendLine("Uchuu#9609 (developer) ([Profile](https://myanimelist.net/profile/Uchuuu))")
                .ToString();

            builder.WithDescription(description);
            await ReplyAsync(embed: builder.Build());
        }
    }
}