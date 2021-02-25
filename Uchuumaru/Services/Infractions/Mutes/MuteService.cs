using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Infractions.Mutes
{
    /// <inheritdoc/> 
    public class MuteService : IMuteService
    {
        /// <summary>
        /// The cached mutes.
        /// </summary>
        private readonly ConcurrentDictionary<int, Timer> _mutes = new();

        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;
        
        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;
        
        /// <summary>
        /// The infraction service.
        /// </summary>
        private readonly IInfractionService _infraction;

        /// <summary>
        /// Constructs a new <see cref="MuteService"/> with the
        /// given injected dependencies.
        /// </summary>
        public MuteService(
            UchuumaruContext uchuumaruContext,
            DiscordSocketClient client,
            IInfractionService infraction)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
            _infraction = infraction;
        }

        /// <inheritdoc/>
        public async Task CreateMute(
            ulong guildId,
            ulong subjectId, 
            ulong moderatorId,
            TimeSpan duration, 
            string reason = null)
        {
            var id = await _infraction.CreateInfraction(InfractionType.MUTE, guildId, subjectId, moderatorId, duration, reason);
            
            var timer = new Timer(async _ => await MuteCallback(guildId, _client.CurrentUser.Id, id),
                null,
                duration,
                Timeout.InfiniteTimeSpan);

            _mutes.TryAdd(id, timer);
         
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();
            
            var socketGuild = _client.GetGuild(guildId);
            var subject = socketGuild.GetUser(subjectId);
            var moderator = socketGuild.GetUser(moderatorId);

            if (subject is not null)
            {
                var muteRole = socketGuild.GetRole(guild.MuteRoleId);
                await subject.AddRoleAsync(muteRole);
            }
            
            // If the channel was never set.
            if (guild.InfractionChannelId == 0)
            {
                return; 
            }
            
            // ReSharper disable once UseNegatedPatternMatching
            var infractionChannel = _client.GetChannel(guild.InfractionChannelId) as ITextChannel;
            
            // If the channel no longer exists.
            if (infractionChannel is null)
            {
                return; 
            }

            var builder = new InfractionEmbedBuilder("User Muted", id, duration, subject, moderator, reason); 
            var message = await infractionChannel.SendMessageAsync(embed: builder.Build());

            if (reason is null)
            {
                builder.Reason = $"+infraction claim {message.Id} <reason>";
            }
            
            await message.ModifyAsync(props => props.Embed = builder.Build());
        }
        
        /// <inheritdoc/>
        public async Task<InfractionSummary> GetActiveMute(ulong guildId, ulong userId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var mute = guild
                .Infractions
                .Where(x => x.Type == InfractionType.MUTE)
                .Where(x => x.Completed == false)
                .FirstOrDefault(x => x.SubjectId == userId);

            return mute?.ToInfractionSummary();
        }
        
        /// <inheritdoc/>
        public async Task CompleteMute(ulong guildId, int id)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();
    
            var mute = guild
                .Infractions
                .Where(x => x.Type is InfractionType.MUTE)
                .FirstOrDefault(x => x.Id == id);
            
            _mutes.TryRemove(mute.Id, out var timer);
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            
            mute.Completed = true;
            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public Task CacheMutes()
        {
            var guilds = _uchuumaruContext
                .Guilds
                .Include(x => x.Infractions);

            foreach (var guild in guilds)
            {
                var mutes = guild
                    .Infractions
                    .Where(x => x.Type == InfractionType.MUTE)
                    .Where(x => !x.Completed)
                    .ToList();
                
                foreach (var mute in mutes)
                {
                    var timer = new Timer(async _ => await MuteCallback(mute.Guild.GuildId, _client.CurrentUser.Id, mute.Id),
                        null,
                        mute.RemainingTime,
                        Timeout.InfiniteTimeSpan);
            
                    _mutes.TryAdd(mute.Id, timer);
                }
            }
            
            return Task.CompletedTask;
        }
        
        /// <inheritdoc/>
        public async Task MuteCallback(ulong guildId, ulong moderatorId, int id)
        {
            // Find the guild.
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Infractions)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            // Find the mute.
            var mute = guild
                .Infractions
                .Where(x => x.Type is InfractionType.MUTE)
                .FirstOrDefault(x => x.Id == id);

            // Complete the mute.
            await CompleteMute(guildId, id);

            // Remove muted role from user.
            var socketGuild = _client.GetGuild(guildId);
            var subject = socketGuild.GetUser(mute.SubjectId);
            var moderator = socketGuild.GetUser(moderatorId);

            if (subject is null)
            {
                return;
            }
            
            var muteRole = socketGuild.GetRole(guild.MuteRoleId);
            await subject.RemoveRoleAsync(muteRole); ;

            // If the channel was never set.
            if (guild.InfractionChannelId == 0)
            {
                return; 
            }
            
            // ReSharper disable once UseNegatedPatternMatching
            var infractionChannel = _client.GetChannel(guild.InfractionChannelId) as ITextChannel;
            
            // If the channel no longer exists.
            if (infractionChannel is null)
            {
                return; 
            }

            var embed = new InfractionEmbedBuilder("User Unmuted", mute.Id, subject, moderator).Build();
            await infractionChannel.SendMessageAsync(embed: embed);
        }
    }
}