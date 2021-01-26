using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Services.Users
{
    /// <summary>
    /// Contains extensions methods for the configuration of user services on application
    /// startup.
    /// </summary>
    public static class UserSetup
    {
        /// <summary>
        /// Adds the user interfaces and classes that make up the guild services.
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns><paramref name="collection"/></returns>
        public static IServiceCollection AddUsers(this IServiceCollection collection)
        {
            return collection.AddSingleton<IUserService, UserService>();
        }
    }
}