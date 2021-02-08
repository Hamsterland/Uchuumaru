using Microsoft.Extensions.DependencyInjection;
using Uchuumaru.Services.Filters;
using Uchuumaru.Services.Infractions.Bans;
using Uchuumaru.Services.Infractions.Mutes;
using Uchuumaru.Services.Infractions.Warns;

namespace Uchuumaru.Services.Infractions
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
            return collection
                .AddSingleton<IFilterService, FilterService>()
                .AddSingleton<IMuteService, MuteService>()
                .AddSingleton<IBanService, BanService>()
                .AddSingleton<IWarnService, WarnService>();
        }
    }
}