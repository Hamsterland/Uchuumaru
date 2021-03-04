using System;
using System.Threading.Tasks;

namespace Uchuumaru.Services.MAL
{
    /// <summary>
    /// Describes a service that verifies a user.
    /// </summary>
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public interface IVerificationService
    {
        /// <summary>
        /// Gets the <see cref="Profile"/> of a verified user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>
        /// A <see cref="Profile"/> of a verified user.
        /// </returns>
        Task<Profile> GetProfile(ulong userId);
        
        /// <summary>
        /// Begins verification. If the user is new, a Verification Code is made for them.
        /// If the user not new, they are sent their existing Verification Code.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task Start(ulong guildId, ulong userId);
        
        /// <summary>
        /// Checks a user's location with their Verification Code and attempts
        /// to verify them.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="username">The MAL username.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task Confirm(ulong userId, string username);
    }
}