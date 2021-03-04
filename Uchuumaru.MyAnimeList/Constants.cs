namespace Uchuumaru.MyAnimeList
{
    /// <summary>
    /// Holds important constants related to MyAnimeList.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The root Url of the MyAnimeList website.
        /// </summary>
        public const string MyAnimeListRootUrl = "https://myanimelist.net/";
        
        /// <summary>
        /// The profile controller's Url
        /// </summary>
        public const string ProfileUrl = "profile/";
        
        /// <summary>
        /// The anime list controller's Url.
        /// </summary>
        public const string AnimeListUrl = "animelist/";
        
        /// <summary>
        /// The manga list controller's Url.
        /// </summary>
        public const string MangaListUrl = "mangalist/";

        /// <summary>
        /// The comments page Url.
        /// </summary>
        public const string CommentsUrl = "comments.php?id={0}";
    }
}