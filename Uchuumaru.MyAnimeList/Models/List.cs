namespace Uchuumaru.MyAnimeList.Models
{
    /// <summary>
    /// A generic account list.
    /// </summary>
    public class List
    {
        /// <summary>
        /// The list Url.
        /// </summary>
        public string Url { get; init; }
        
        /// <summary>
        /// The list mean score.
        /// </summary>
        public double MeanScore { get; init; }
    }
}