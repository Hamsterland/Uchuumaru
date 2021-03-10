using Discord;

namespace Uchuumaru
{
    /// <summary>
    /// Application-wide constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The default colour theme.
        /// </summary>
        public static Color DefaultColour { get; } = new(252, 190, 253);

        /// <summary>
        /// MyAnimeList Guild Id. 
        /// </summary>
        public const ulong MyAnimeListId = 301123999000166400;

        /// <summary>
        /// The verified role Id.
        /// </summary>
        public const ulong VerifiedId = 372178027926519810;
    }
}