using System;

namespace Uchuumaru.Services.MAL
{
    public class Profile
    {
        public readonly string Name;
    
        public Profile(string name)
        {
            Name = name;
        }

        public string ProfileUrl { get; set; }
        public string AnimeListUrl { get; set; }
        public string MangaListUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool MalSupporter { get; set; }
        public double Score { get; set; }
        public string LastOnline { get; set; }
        public Gender Gender { get; set; }
        public string Birthday { get; set; }
        public string Location { get; set; }
        public DateTime DateJoined { get; set; }
    }

    public enum Gender
    {
        MALE,
        FEMALE,
        NONBINARY,
        UNSPECIFIED
    }
}