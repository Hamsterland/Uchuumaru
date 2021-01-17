using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Filters
{
    /// <inheritdoc/>
    public class FilterService : IFilterService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="FilterService"/> with the
        /// given injected dependencies.
        /// </summary>
        /// <param name="uchuumaruContext"></param>
        public FilterService(UchuumaruContext uchuumaruContext, DiscordSocketClient client)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
        }

        /// <inheritdoc/>
        public async Task ModifyFilter(ulong guildId, bool enabledFitler)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync();

            _ = guild ?? throw new EntityNotFoundException<Guild>();

            guild.EnabledFilter = enabledFitler;
            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ModifyFilterChannel(FilterChannelOptions options, ulong guildId, ulong channelId = 0)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();
            
            if (options == FilterChannelOptions.Set)
            {
                var channel = _client.GetChannel(channelId) as IGuildChannel;

                if (channel is not ITextChannel )
                {
                    throw new InvalidFilterChannelException(channel.Name, channelId); 
                }

                guild.FilterChannelId = channelId;
            }
            else
            {
                guild.FilterChannelId = 0;
            }

            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<FilterStatus> GetFilterStatus(ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .AsNoTracking()
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.GuildId == guildId);
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var expressions = guild
                .Filters
                .Select(x => x.Expression)
                .ToList();

            return new FilterStatus
            {
                GuildId = guild.GuildId,
                FilterChannelId = guild.FilterChannelId,
                Enabled = guild.EnabledFilter,
                Expressions = expressions
            };
        }

        /// <inheritdoc/>
        public async Task AddFilter(ulong guildId, string expression)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            if (guild.Filters.Any(x => x.Expression == expression))
            {
                throw new ExpressionExistsException(expression);
            }
            
            guild.Filters.Add(new Filter
            {
                Expression = expression
            });

            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveFilter(ulong guildId, string expression)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .Where(x => x.GuildId == guildId)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync();
            
            _ = guild ?? throw new EntityNotFoundException<Guild>();

            if (guild.Filters.All(x => x.Expression != expression))
            {
                throw new ExpressionDoesNotExistException(expression);
            }

            guild
                .Filters
                .RemoveAll(x => x.Expression == expression);
            
            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}