using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Infractions.Bans
{
    public class BanService : IBanService
    {
        private readonly UchuumaruContext _uchuumaruContext;

        private readonly DiscordSocketClient _client;
        
        public BanService(UchuumaruContext uchuumaruContext, DiscordSocketClient client)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
        }

        public async Task Unban(ulong guildId, ulong subjectId, ulong moderatorId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var ban = guild
                .Infractions
                .Where(x => x.Type == InfractionType.BAN)
                .Where(x => x.Completed == false)
                .FirstOrDefault(x => x.SubjectId == subjectId);

            ban.Completed = true;
            await _uchuumaruContext.SaveChangesAsync();
            
            if (guild.InfractionChannelId == 0)
            {
                return; 
            }
            
            // ReSharper disable once UseNegatedPatternMatching
            var infractionChannel = _client.GetChannel(guild.InfractionChannelId) as ITextChannel;
            
            if (infractionChannel is null)
            {
                return; 
            }

            var subject = await _client.Rest.GetUserAsync(subjectId);
            var moderator = _client.GetUser(moderatorId);
            
            var builder = new InfractionEmbedBuilder("User Unbanned", ban.Id, subject, moderator); 
            await infractionChannel.SendMessageAsync(embed: builder.Build());
        }
    }
}