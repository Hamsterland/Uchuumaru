using System;

namespace Uchuumaru.Services.MAL
{
    /// <summary>
    /// The profile of a MAL user.
    /// </summary>
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public class Profile
    {
        /// <summary>
        /// The username.
        /// </summary>
        public string Name { get; }
    
        /// <summary>
        /// Constructs a new <see cref="Profile"/>.
        /// </summary>
        /// <param name="name">The username.</param>
        public Profile(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The ID of the user.
        /// </summary>
        public int ID { get; init; }
        
        /// <summary>
        /// The URL to the profile.
        /// </summary>
        public string ProfileUrl { get; init; }
        
        /// <summary>
        /// The URL to the Anime list.
        /// </summary>
        public string AnimeListUrl { get; init; }
        
        /// <summary>
        /// The URL to the Manga list.
        /// </summary>
        public string MangaListUrl { get; init; }
        
        /// <summary>
        /// The URL to the image.
        /// </summary>
        public string ImageUrl { get; init; }
        
        /// <summary>
        /// Whether the user is a MAL Supporter.
        /// </summary>
        public bool IsSupporter { get; init; }
        
        /// <summary>
        /// The mean anime score.
        /// </summary>
        public double MeanScore { get; init; }
        
        /// <summary>
        /// The time the user was last online.
        /// </summary>
        public string RawLastOnline { get; init; }
        
        /// <summary>
        /// The gender of the user.
        /// </summary>
        public Gender Gender { get; init; }

        /// <summary>
        /// A string representation of the user's <see cref="Gender"/>.
        /// </summary>
        public string FormattedGender => Gender switch
        {
            Gender.MALE => "Male",
            Gender.FEMALE => "Female",
            Gender.NONBINARY => "Non-Binary",
            Gender.UNSPECIFIED => "Unspecified",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        /// <summary>
        /// The emote associated with the user's <see cref="Gender"/>.
        /// </summary>
        public string GenderEmote => Gender switch
        {
            Gender.MALE => ":male_sign:",
            Gender.FEMALE => ":female_sign:",
            Gender.NONBINARY => ":regional_indicator_o:",
            Gender.UNSPECIFIED => ":question:",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        /// <summary>
        /// The birthday of the user.
        /// </summary>
        public string RawBirthday { get; init; }
        
        /// <summary>
        /// The location of the user.
        /// </summary>
        public string Location { get; init; }
        
        /// <summary>
        /// The time the user joined.
        /// </summary>
        public DateTime DateJoined { get; init; }
    }

    /// <summary>
    /// Represents a possible gender.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Male.
        /// </summary>
        MALE,
        
        /// <summary>
        /// Female.
        /// </summary>
        FEMALE,
        
        /// <summary>
        /// Non-binary.
        /// </summary>
        NONBINARY,
        
        /// <summary>
        /// Unknown.
        /// </summary>
        UNSPECIFIED
    }
}