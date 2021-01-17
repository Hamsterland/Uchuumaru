using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

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
        /// Constructs a new <see cref="InfractionService"/> with the given
        /// injected dependencies.
        /// </summary>
        public InfractionService(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <inheritdoc/>
        public async Task CreateInfraction(
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
        }
    }
}