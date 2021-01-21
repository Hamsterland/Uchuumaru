using System.Threading.Tasks;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// Describes a service that operates a content filter.
    /// </summary>
    public interface IFilterService
    {
        /// <summary>
        /// Disable or enables the filter for a guild. If the guild cannot be found, a
        /// <see cref="EntityNotFoundException{TEntity}"/> will be thrown.
        /// </summary>
        /// <param name="guildId">The target guild Id.</param>
        /// <param name="enabledFitler">Whether the filter should be enabled or disabled.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task ModifyFilter(ulong guildId, bool enabledFitler);

        /// <summary>
        /// Sets or removes the <see cref="Guild.FilterChannelId"/> property in the database.
        /// Throws an <see cref="InvalidFilterChannelException"/> if the specified channel is
        /// not an <see cref="Discord.ITextChannel"/>.
        /// </summary>
        /// <param name="modificationOptions">Whether to set or remove.</param>
        /// <param name="guildId">The target Guild Id.</param>
        /// <param name="channelId">The target channel Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task ModifyFilterChannel(ChannelModificationOptions modificationOptions, ulong guildId, ulong channelId = 0);

        /// <summary>
        /// Gets the current status of the filter for a guild.  If the guild cannot be found, a
        /// <see cref="EntityNotFoundException{TEntity}"/> will be thrown.
        /// </summary>
        /// <param name="guildId">The target guild Id.</param>
        /// <returns>
        /// The current filter status.
        /// </returns>
        Task<FilterStatus> GetFilterStatus(ulong guildId);
        
        /// <summary>
        /// Adds and expression to the filter for a guild. If the <paramref name="expression"/>
        /// already exists, an <see cref="ExpressionExistsException"/> will be thrown. If the guild
        /// cannot be found, a <see cref="EntityNotFoundException{TEntity}"/> will be thrown.
        /// </summary>
        /// <param name="guildId">The target guild Id.</param>
        /// <param name="expression">The expression to be added.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task AddFilter(ulong guildId, string expression);
        
        /// <summary>
        /// Removes an expression from the filter for a guild. If the <paramref name="expression"/>
        /// does not exist, an <see cref="ExpressionDoesNotExistException"/> will be thrown. If the guild
        /// cannot be found, a <see cref="EntityNotFoundException{TEntity}"/> will be thrown.
        /// </summary>
        /// <param name="guildId">The target guild Id.</param>
        /// <param name="expression">The expression to be removed.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task RemoveFilter(ulong guildId, string expression);
    }
}