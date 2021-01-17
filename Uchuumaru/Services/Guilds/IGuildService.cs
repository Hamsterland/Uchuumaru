using System.Threading.Tasks;

namespace Uchuumaru.Services.Guilds
{
    /// <summary>
    /// Describes a service that manages guilds stored in the database.
    /// </summary>
    public interface IGuildService
    {
        /// <summary>
        /// Adds a guild to the database. If a guild with the same <paramref name="guildId"/> already
        /// exists, this guild is deleted and a new one is added.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task AddGuild(ulong guildId);
        
        /// <summary>
        /// Removes a guild from the database using the guild Id. If a guild with the specified
        /// <paramref name="guildId"/> does not exist, the function returns.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task RemoveGuild(ulong guildId);
        
        /// <summary>
        /// Removes a guild from the database using an Id. If a guild with the specified
        /// <paramref name="id"/> does not exist, the function returns.
        /// </summary>
        /// <param name="id">The database primary key.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task RemoveGuild(int id);
    }
}