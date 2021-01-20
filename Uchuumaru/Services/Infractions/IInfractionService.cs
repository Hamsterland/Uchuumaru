using System;
using System.Threading.Tasks;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Infractions
{
    /// <summary>
    /// Describes a service that manages infractions, particularly database-side.
    /// </summary>
    public interface IInfractionService
    {
        /// <summary>
        /// Creates a new infraction.
        /// </summary>
        /// <param name="type">The type of infraction.</param>
        /// <param name="guildId">The guild the infraction belongs to.</param>
        /// <param name="subjectId">The user subject to the infraction.</param>
        /// <param name="moderatorId">The moderator performing the infraction.</param>
        /// <param name="duration">The duration of the infraction.</param>
        /// <param name="reason">The reason for the infraction.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task<int> CreateInfraction(
            InfractionType type, 
            ulong guildId, 
            ulong subjectId, 
            ulong moderatorId, 
            TimeSpan duration, 
            string reason = null);

        /// <summary>
        /// Retrieves the <see cref="Guild.InfractionChannelId"/> property value. 
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <returns>
        /// The Id of the guild infraction channel. 
        /// </returns>
        Task<ulong> GetInfractionChannelId(ulong guildId);
    }
}