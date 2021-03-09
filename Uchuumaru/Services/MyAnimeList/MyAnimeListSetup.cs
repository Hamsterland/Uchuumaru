using Microsoft.Extensions.DependencyInjection;
using Uchuumaru.MyAnimeList.Parsers;

namespace Uchuumaru.Services.MyAnimeList
{
    public static class MyAnimeListSetup
    {
        public static IServiceCollection AddMyAnimeList(this IServiceCollection collection)
        {
            return collection
                .AddScoped<ProfileParser>()
                .AddScoped<CommentsParser>()
                .AddScoped<IActivityService, ActivityService>()
                .AddScoped<IVerificationService, VerificationService>();
        }
    }
}