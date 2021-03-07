using Discord;
using Uchuumaru.MyAnimeList.Models;

namespace Uchuumaru.Services.MyAnimeList
{
    /// <summary>
    /// An <see cref="EmbedBuilder"/> to create MyAnimeList profiles.
    /// </summary>
    public class ProfileEmbedBuilder : EmbedBuilder
    {
        /// <summary>
        /// The profile.
        /// </summary>
        public Profile Profile { get; }

        /// <summary>
        /// Constructs a new <see cref="ProfileEmbedBuilder"/>.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public ProfileEmbedBuilder(Profile profile)
        {
            Profile = profile;
            WithColor(Constants.DefaultColour);
        }

        private readonly Emoji _alarmclock = new("\u23F0");
        private readonly Emoji _maleSign = new("♂️");
        private readonly Emoji _femaleSign = new("♀️");
        private readonly Emoji _nonBinary = new(":zero:");
        private readonly Emoji _unspecified = new("\u2753");
        private readonly Emoji _date = new("\uD83D\uDCC5");
        private readonly Emoji _map = new("🗺️");
        private readonly Emoji _hourGlass = new("\u23F3");
        private readonly Emoji _barChart = new("\uD83D\uDCCA");
        
        public ProfileEmbedBuilder WithName()
        {
            WithTitle($"{Profile.Username}'s Profile");
            WithUrl(Profile.Url);
            return this;
        }

        public ProfileEmbedBuilder WithProfileImage()
        {
            WithThumbnailUrl(Profile.ImageUrl);
            return this;
        }

        public ProfileEmbedBuilder WithListUrls()
        {
            WithDescription($"[Anime List]({Profile.AnimeList.Url}) • [Manga List]({Profile.MangaList.Url})");
            return this;
        }
        
        public ProfileEmbedBuilder WithLastOnline(bool inline = true)
        {
            AddField($"{_alarmclock} Last Online", Profile.LastOnline, inline);
            return this;
        }

        public ProfileEmbedBuilder WithGender(bool inline = true)
        {
#pragma warning disable 8509
            var emote = Profile.Gender switch
#pragma warning restore 8509
            {
                Gender.Male => _maleSign,
                Gender.Female => _femaleSign,
                Gender.NonBinary => _nonBinary,
                Gender.Unspecified => _unspecified
            };

            AddField($"{emote} Gender", Profile.Gender, inline);
            return this;
        }

        public ProfileEmbedBuilder WithBirthday(string backupText = "No Birthday", bool inline = true)
        {
            AddField($"{_date} Birthday", Profile.Birthday ?? backupText, inline);
            return this;
        }

        public ProfileEmbedBuilder WithLocation(string backupText = "No Location", bool inline = true)
        {
            AddField($"{_map} Location", Profile.Location ?? backupText, inline);
            return this;
        }

        public ProfileEmbedBuilder WithDateJoined(bool inline = true)
        {
            AddField($"{_hourGlass} Joined", $"{Profile.DateJoined:D}", inline);
            return this;
        }

        public ProfileEmbedBuilder WithMeanScore(bool inline = true)
        {
            AddField($"{_barChart} Mean Score", Profile.AnimeList.MeanScore, inline);
            return this;
        }

        public Embed BuildFullEmbed()
        {
            return new ProfileEmbedBuilder(Profile)
                .WithName()
                .WithProfileImage()
                .WithListUrls()
                .WithLastOnline()
                .WithGender()
                .WithBirthday()
                .WithLocation()
                .WithDateJoined()
                .WithMeanScore()
                .Build();
        }
    }
}