using System.Linq;
using System.Threading.Tasks;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Guilds
{
    /// <inheritdoc/>
    public class GuildService : IGuildService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="GuildService"/> with the given
        /// injected dependencies.
        /// </summary>
        public GuildService(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <inheritdoc/>
        public async Task AddGuild(ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (guild is not null)
            {
                await RemoveGuild(guild.Id);
            }

            _uchuumaruContext.Add(new Guild
            {
                GuildId = guildId,
                EnabledFilter = false
            });

            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveGuild(ulong guildId)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (guild is null)
            {
                return; 
            }
            
            _uchuumaruContext
                .Guilds
                .Remove(guild);

            await _uchuumaruContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveGuild(int id)
        {
            var guild = await _uchuumaruContext
                .Guilds
                .FindAsync(id);

            if (guild is null)
            {
                return; 
            }

            _uchuumaruContext
                .Guilds
                .Remove(guild);

            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}