using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// Contains extensions methods for the configuration of infraction services on application
    /// startup.
    /// </summary>
    public static class InfractionSetup
    {
        /// <summary>
        /// Adds the infraction interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddInfractions(this IServiceCollection collection)
        {
            return collection.AddSingleton<IFilterService, FilterService>();
        }
    }
}