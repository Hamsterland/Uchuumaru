using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Uchuumaru.Preconditions;

namespace Uchuumaru.Modules
{
    [Name("Text")]
    [Summary("Text manipulation.")]
    public class TextModule : ModuleBase<SocketCommandContext>
    {
        private readonly Regex _buildContentRegex = new(@"```([^\s]+|)");
        
        [Command("say")]
        [Alias("echo")]
        [Summary("Echoes a message.")]
        [RequireModeratorOrDeveloper]
        public async Task Echo([Remainder] string text)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(text);
        }
        
        [Command("embed")]
        [Summary("Created an embed.")]
        [RequireModeratorOrDeveloper]
        public async Task Embed([Remainder] string json)
        {
            var cleanCode = _buildContentRegex
                .Replace(json.Trim(), string.Empty)
                .Replace("\t", "    ");
            
            var builder = JsonConvert.DeserializeObject<EmbedBuilder>(cleanCode);
            var embed = builder.Build();

            await ReplyAsync(embed: embed);
        }
    }
}