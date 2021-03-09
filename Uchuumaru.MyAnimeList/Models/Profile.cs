using System;
using System.Threading.Tasks;
using Uchuumaru.MyAnimeList.Parsers;
using static Uchuumaru.MyAnimeList.Constants;

namespace Uchuumaru.MyAnimeList.Models
{
    /// <summary>
    /// A MyAnimeList account profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// The username.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// The MyAnimeList Id.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The profile Url.
        /// </summary>
        public string Url { get; init; }

        /// <summary>
        /// The account image Url.
        /// </summary>
        public string ImageUrl { get; init; }

        /// <summary>
        /// The date the user joined MyAnimeList.
        /// </summary>
        public DateTime DateJoined { get; init; }

        /// <summary>
        /// The gender.
        /// </summary>
        public Gender Gender { get; init; }

        /// <summary>
        /// The anime list.
        /// </summary>
        public List AnimeList { get; set; }

        /// <summary>
        /// The manga list.
        /// </summary>
        public List MangaList { get; set; }

        /// <summary>
        /// Whether the account has MAL Supporter.
        /// </summary>
        public bool IsSupporter { get; init; }

        /// <summary>
        /// A <see cref="string"/> showing when the user was last online.
        /// </summary>
        /// <remarks>
        /// This is not a <see cref="DateTime"/> because parsing is too difficult.
        /// </remarks>
        public string LastOnline { get; init; }

        /// <summary>
        /// A <see cref="string"/> showing the user's birthday.
        /// </summary>
        /// <remarks>
        /// This is not a <see cref="DateTime"/> because parsing is too difficult.
        /// </remarks>
        public string Birthday { get; init; }

        /// <summary>
        /// The location.
        /// </summary>
        public string Location { get; init; }

        /// <summary>
        /// Creates a <see cref="Profile"/> from a MyAnimeList user's account Url.
        /// </summary>
        /// <param name="username">The MyAnimeList account username.</param>
        /// <param name="parser">The <see cref="ProfileParser"/> instance to use.</param>
        /// <returns>
        /// A <see cref="Profile"/> created from a MyAnimeList user's account Url.
        /// </returns>
        public static async Task<Profile> FromUsername(string username, ProfileParser parser)
        {
            await parser.Refresh(username);

            return new Profile
            {
                Username = username,
                Id = parser.GetUserId(),
                Url = MyAnimeListRootUrl + ProfileUrl + username,
                ImageUrl = parser.GetImageUrl(),
                DateJoined = parser.GetDateJoined(),
                Gender = parser.GetGender(),
                IsSupporter = parser.GetSupporter(),
                LastOnline = parser.GetLastOnline(),
                Birthday = parser.GetBirthday(),
                Location = parser.GetLocation(),

                AnimeList = new List
                {
                    Url = MyAnimeListRootUrl + AnimeListUrl + username,
                    MeanScore = parser.GetAnimeMeanScore()
                },
                
                MangaList = new List 
                {
                    Url = MyAnimeListRootUrl + MangaListUrl + username,
                    MeanScore = 0
                }
            };
        }
    }
}