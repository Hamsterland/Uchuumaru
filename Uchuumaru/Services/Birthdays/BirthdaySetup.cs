using Microsoft.Extensions.DependencyInjection;
using Uchuumaru.Services.Guilds;

namespace Uchuumaru.Services.Birthdays
{
    /// <summary>
    /// Contains extensions methods for the configuration of birthday services on application
    /// startup.
    /// </summary>
    public static class BirthdaySetup
    {
        /// <summary>
        /// Adds the birthday interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddBirthdays(this IServiceCollection collection)
        {
            return collection.AddSingleton<IBirthdayService, BirthdayService>();
        }
    }
}