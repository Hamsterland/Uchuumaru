using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Infractions.Reports
{
    /// <summary>
    /// Contains extensions methods for the configuration of report services on application
    /// startup.
    /// </summary>
    public static class ReportSetup
    {
        /// <summary>
        /// Adds the report interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddReports(this IServiceCollection collection)
        {
            return collection.AddSingleton<IReportService, ReportService>();
        }
    }
}