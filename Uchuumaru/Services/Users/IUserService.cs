using System.Threading.Tasks;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Users
{
    /// <summary>
    /// Describes a service that performs user operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets information about a user.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="userId"></param>
        /// <returns>
        /// A <see cref="UserSummary"/> or null if the user
        /// is not found.
        /// </returns>
        Task<UserSummary> GetUserSummary(ulong guildId, ulong userId);
    }
}