using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// Contains extensions methods for the configuration of filter services on application
    /// startup.
    /// </summary>
    public static class FilterSetup
    {
        /// <summary>
        /// Adds the guild interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddFilter(this IServiceCollection collection)
        {
            return collection.AddSingleton<IFilterService, FilterService>();
        }
    }
}