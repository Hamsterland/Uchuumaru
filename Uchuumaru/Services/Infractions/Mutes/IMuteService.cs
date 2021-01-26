using System;
using System.Threading.Tasks;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Infractions.Mutes
{
    /// <summary>
    /// Describes a service that performs mute operations.
    /// </summary>
    public interface IMuteService
    {
        /// <summary>
        /// Creates a new mute infraction.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="subjectId">The subject Id.</param>
        /// <param name="moderatorId">The moderator Id.</param>
        /// <param name="duration">The mute duration/</param>
        /// <param name="reason">The mute reason.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task CreateMute(
            ulong guildId,
            ulong subjectId,
            ulong moderatorId,
            TimeSpan duration,
            string reason = null);
        
        /// <summary>
        /// Gets a user's active mute.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>
        /// An <see cref="InfractionSummary"/> representation of the active
        /// mute if there is one. If there is not, null is returned.
        /// </returns>
        Task<InfractionSummary> GetActiveMute(ulong guildId, ulong userId);
        
        /// <summary>
        /// Completes a mute.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="id">The mute Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task CompleteMute(ulong guildId, int id);
        
        /// <summary>
        /// Caches all mutes from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task CacheMutes();
        
        /// <summary>
        /// Handles the callback of a mute expiration.
        /// 1) Finds the mute.
        /// 2) Completes the mute.
        /// 3) Removes the mute role from the subject.
        /// 4) Finds the infraction channel.
        /// 5) Sends a user unmuted message.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="id">The mute Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task MuteCallback(ulong guildId, int id);
    }
}