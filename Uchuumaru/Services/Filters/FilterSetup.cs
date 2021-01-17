using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Infractions
{
    /// <summary>
    /// Contains extensions methods for the configuration of filter services on application
    /// startup.
    /// </summary>
    public static class FilterSetup
    {
        /// <summary>
        /// Adds the filter interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddFilter(this IServiceCollection collection)
        {
            return collection.AddSingleton<IInfractionService, InfractionService>();
        }
    }
}