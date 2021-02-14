using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Infractions.Warns
{
    public class WarnService : IWarnService
    {
        private readonly IInfractionService _infraction;
        private readonly DiscordSocketClient _client;
        private readonly UchuumaruContext _uchuumaruContext;
        
        public WarnService(
            IInfractionService infraction, 
            DiscordSocketClient client, 
            UchuumaruContext uchuumaruContext)
        {
            _infraction = infraction;
            _client = client;
            _uchuumaruContext = uchuumaruContext;
        }

        public async Task Create(
            ulong guildId,
            ulong subjectId,
            ulong moderatorId,
            bool log,
            string reason = null)
        {
            var id = await _infraction.CreateInfraction(
                InfractionType.WARNING, 
                guildId, subjectId, 
                moderatorId, 
                TimeSpan.Zero,
                reason);
            
            if (!log)
                return;
            
            var infractionChannelId = await _infraction.GetInfractionChannelId(guildId);
            
            if (infractionChannelId == 0)
                return;

            if (_client.GetChannel(infractionChannelId) is not ITextChannel infractionChannel)
                return;

            var user = _client.GetUser(subjectId);
            var moderator = _client.GetUser(moderatorId);
            
            var builder = new InfractionEmbedBuilder("User Warned", id, user, moderator, reason);
            var message = await infractionChannel.SendMessageAsync(embed: builder.Build());

            if (reason is null)
                builder.Reason = $"+infraction claim {message.Id} <reason>";
            
            await message.ModifyAsync(props => props.Embed = builder.Build());
        }

        public async Task Rescind(int id, ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            if (guild is null)
                throw new EntityNotFoundException<Guild>();

            var warn = guild
                .Infractions
                .Find(infraction => infraction.Id == id);

            if (warn is null)
                throw new InfractionNotFoundException(id);

            warn.Completed = true;
            await _uchuumaruContext.SaveChangesAsync();
        }

        public async Task Delete(int id, ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            if (guild is null)
                throw new EntityNotFoundException<Guild>();

            var warn = guild
                .Infractions
                .Find(infraction => infraction.Id == id);

            if (warn is null)
                throw new InfractionNotFoundException(id);

            guild.Infractions.Remove(warn);
            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}