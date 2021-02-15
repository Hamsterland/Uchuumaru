using Discord;

namespace Uchuumaru.Utilities
{
    public static class UserExtensions
    {
        public static string Represent(this IUser user) => $"{user.Mention} ({user.Id})";
    }
}