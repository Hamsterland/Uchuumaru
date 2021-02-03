using System.Threading.Tasks;

namespace Uchuumaru.Services.Infractions.Reports
{
    /// <summary>
    /// Describes a service that handles the reporting feature.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Sends a report to the moderators.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="channelId">The channel Id.</param>
        /// <param name="guildId">The Guild Id.</param>
        /// <param name="report">The report content.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task Report(ulong userId, ulong channelId, ulong guildId, string report);
    }
}