using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;
using Uchuumaru.Services.Filters;

namespace Uchuumaru.Services.Infractions
{
    /// <inheritdoc/>
    public class InfractionService : IInfractionService
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="InfractionService"/> with the given
        /// injected dependencies.
        /// </summary>
        public InfractionService(UchuumaruContext uchuumaruContext, DiscordSocketClient client)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
        }

        /// <inheritdoc/>
        public async Task<int> CreateInfraction(
            InfractionType type, 
            ulong guildId, 
            ulong subjectId, 
            ulong moderatorId, 
            TimeSpan duration, 
            string reason = null)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var infraction = new Infraction
            {
                Type = type,
                SubjectId = subjectId,
                ModeratorId = moderatorId,
                Duration = duration,
                Reason = reason,
                Completed = false
            };
            
            guild.Infractions.Add(infraction);
            await _uchuumaruContext.SaveChangesAsync();
            return infraction.Id;
        }

        /// <inheritdoc/>
        public async Task ModifyInfractionChannel(ChannelModificationOptions options, ulong guildId, ulong channelId = 0)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();
            
            if (options == ChannelModificationOptions.Set)
            {
                var channel = _client.GetChannel(channelId) as IGuildChannel;

                if (channel is not ITextChannel)
                {
                    throw new InvalidInfractionChannelException(channel.Name, channelId); 
                }

                guild.InfractionChannelId = channelId;
            }
            else
            {
                guild.InfractionChannelId = 0;
            }

            await _uchuumaruContext.SaveChangesAsync();
        }
        
        /// <inheritdoc/>
        public async Task<ulong> GetInfractionChannelId(ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            return guild.InfractionChannelId; 
        }

        /// <inheritdoc/>
        public async Task<IUserMessage> GetInfractionMessage(ulong guildId, ulong messageId)
        {
            var guild = _client.GetGuild(guildId); 
            var infractionChannelId = await GetInfractionChannelId(guildId);
            var infractionChannel = guild.GetChannel(infractionChannelId) as ITextChannel;
            return await infractionChannel.GetMessageAsync(messageId) as IUserMessage;
        }
        
        /// <inheritdoc/>
        public async Task ClaimInfraction(ulong guildId, ulong messageId, ulong moderatorId, string reason = null)
        {
            var message = await GetInfractionMessage(guildId, messageId);
            _ = message ?? throw new InfractionMessageNotFoundException(messageId);
            
            var id = 0;
            
            await message.ModifyAsync(props =>
            {
                var moderator = _client
                    .GetGuild(guildId)
                    .GetUser(moderatorId);
                
                var builder = new InfractionEmbedBuilder(message.Embeds.FirstOrDefault())
                {
                    Moderator = $"{moderator} ({moderator.Id})",
                };

                if (reason is not null)
                {
                    builder.Reason = reason;
                }
                
                props.Embed = builder.Build();
                id = builder.Case;
            });

            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();

            var infraction = guild
                .Infractions
                .FirstOrDefault(x => x.Id == id);

            infraction.ModeratorId = moderatorId;

            if (reason is not null)
            {
                infraction.Reason = reason;
            }
            
            await _uchuumaruContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InfractionSummary>> GetAllInfractions(ulong guildId, ulong userId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            return guild
                .Infractions
                .Where(x => x.SubjectId == userId)
                .Select(infraction => infraction.ToInfractionSummary()); 
        }
    }
}