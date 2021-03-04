using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Uchuumaru.Services.MAL
{
    /// <inheritdoc/>
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public class ProfileParser : IProfileParser
    {
        private readonly HttpClient _httpClient = new();
        private readonly HtmlParser _htmlParser = new();

        private readonly Regex _imageUrl = new("https://cdn.myanimelist.net/images/userimages/(?<userId>[0-9]+).(png|jp(e)?g|gif)");
        private readonly Regex _usernameComments = new("(?<Username>(.?)+)'s Comments");

        private const string MALRoot = "https://myanimelist.net/";
        private const string Profile = "profile/";
        private const string AnimeList = "animelst/";
        private const string MangaList = "mangalist/";
        
        /// <inheritdoc/>
        public async Task<Profile> Parse(string username)
        {
            var html = await DownloadHtml(username);
            var document = await _htmlParser.ParseDocumentAsync(html);
            var elements = GetClearfixElements(document).ToList();

            var imageUrl = GetImageUrl(document);
            
            return new Profile(username)
            {
                ProfileUrl = MALRoot + Profile + username,
                AnimeListUrl = MALRoot + AnimeList + username,
                MangaListUrl = MALRoot + MangaList + username,
                ImageUrl = imageUrl,
                IsSupporter = GetSupporter(document),
                MeanScore = GetScore(document),
                RawLastOnline = GetLastOnline(elements),
                Gender = GetGender(elements),
                RawBirthday = GetBirthday(elements),
                Location = GetLocation(elements),
                DateJoined = GetDateJoined(elements),
                ID = GetUserId(imageUrl)
            };
        }

        /// <inheritdoc/>
        public async Task<IHtmlDocument> ParseDocumentAsync(string html) 
            => await _htmlParser.ParseDocumentAsync(html);
        
        /// <inheritdoc/>
        public async Task<string> DownloadHtml(string username)
            => await _httpClient
                .GetStringAsync(MALRoot + Profile + username);

        /// <inheritdoc/>
        public bool IsElementSpecified(IEnumerable<IElement> elements, string argument)
            => elements.Any(x => x.InnerHtml
                .Contains(argument));

        /// <inheritdoc/>
        public async Task<string> GetUsernameFromId(int id)
        {
            var url = $"https://myanimelist.net/comments.php?id={id}";
            var html = await _httpClient.GetStringAsync(url);
            var document = await _htmlParser.ParseDocumentAsync(html);

            var content = document
                .GetElementById("contentWrapper")
                .FirstChild
                .TextContent;

            return _usernameComments
                .Matches(content)
                .FirstOrDefault()
                .Groups["Username"]
                .Value;
        }
        
        /// <inheritdoc/>
        public string GetImageUrl(IDocument document)
            => _imageUrl
                .Match(document.Images
                    .FirstOrDefault(x => x.OuterHtml
                        .Contains("https://cdn.myanimelist.net/images/userimages/"))
                    .OuterHtml)
                .Value;

        /// <inheritdoc/>
        public int GetUserId(string imageUrl)
        {
            var matches = _imageUrl.Matches(imageUrl);
            
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups["userId"].Value, out var id))
                    return id;
            }

            return 0;
        }

        /// <inheritdoc/>
        public bool GetSupporter(IDocument document)
            => document
                .All
                .First(x => x.ClassName == "link")
                .Descendents()
                .FirstOrDefault()
                .TextContent == "Supporter";

        /// <inheritdoc/>
        public double GetScore(IDocument document)
        {
            var nodes = document.All.ToArray();
            double score = 0;
            
            for (var i = 10; i > 0; i--)
            {
                var node = nodes.FirstOrDefault(x => x.ClassName == $"score-label score-{i}");

                if (node is null)
                    continue;
                
                if (nodes.HasClass($"score-label score-{i}"))
                    score = double.Parse(nodes.FirstOrDefault(x => x.ClassName == $"score-label score-{i}")
                        .TextContent);
            }
            return score;
        }

        /// <inheritdoc/>
        public IEnumerable<IElement> GetClearfixElements(IDocument document)
            => document
                .All
                .Where(x => x.ClassName == "clearfix");

        /// <inheritdoc/>
        public string GetLastOnline(IEnumerable<IElement> elements)
            => elements
                .FirstOrDefault()
                .Children[1]
                .TextContent;

        /// <inheritdoc/>
        public Gender GetGender(List<IElement> elements)
        {
            if (!IsElementSpecified(elements, "Gender"))
                return Gender.UNSPECIFIED;

            var gender = elements[1]
                .Children[1]
                .TextContent;

            return gender switch
            {
                "Non-Binary" => Gender.NONBINARY,
                "Female" => Gender.FEMALE,
                "Male" => Gender.MALE,
                _ => Gender.UNSPECIFIED
            };
        }

        /// <inheritdoc/>
        public string GetBirthday(List<IElement> elements)
        {
            var index = 2;
            if (!IsElementSpecified(elements, "Gender"))
                index = 1;

            
            return IsElementSpecified(elements, "Birthday")
                ? elements[index]
                    .Children[1]
                    .TextContent
                : null;
        }

        /// <inheritdoc/>
        public string GetLocation(List<IElement> elements)
        {
            return IsElementSpecified(elements, "Location")
                ? elements.ToList()[^3]
                    .Children[1]
                    .TextContent
                : "No Location";
        }

        /// <inheritdoc/>
        public DateTime GetDateJoined(List<IElement> elements)
            => DateTime.Parse(elements[^2]
                .Children[1]
                .TextContent);
    }
}