using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Guilds
{
    /// <summary>
    /// Contains extensions methods for the configuration of guild services on application
    /// startup.
    /// </summary>
    public static class GuildSetup
    {
        /// <summary>
        /// Adds the guild interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddGuild(this IServiceCollection collection)
        {
            return collection.AddSingleton<IGuildService, GuildService>();
        }
    }
}