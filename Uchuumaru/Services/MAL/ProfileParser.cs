using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Uchuumaru.Services.MAL
{
    public class ProfileParser
    {
        private readonly HttpClient _client = new();
        private readonly HtmlParser _parser = new();

        private readonly Regex ImageUrl = new(@"\b(?:https?://|www\.)\S+\b");

        public async Task<Profile> ParseProfile(string username)
        {
            var html = await DownloadHtml(username);
            var document = await _parser.ParseDocumentAsync(html) as IDocument;
            var clearfixElements = GetClearfixElements(document).ToList();

            return new Profile(username)
            {
                ProfileUrl = $"https://myanimelist.net/profile/{username}",
                AnimeListUrl = $"https://myanimelist.net/animelist/{username}",
                MangaListUrl = $"https://myanimelist.net/mangalist/{username}",
                ImageUrl = GetImageUrl(document),
                MalSupporter = IsMalSupporter(document),
                Score = GetScore(document),
                LastOnline = GetLastOnline(clearfixElements),
                Gender = GetGender(clearfixElements),
                Birthday = GetBirthday(clearfixElements),
                Location = GetLocation(clearfixElements),
                DateJoined = GetDateJoined(clearfixElements)
            };
        }
        
        public async Task<string> DownloadHtml(string username)
            => await _client
                .GetStringAsync($"https://myanimelist.net/profile/{username}");

        public static bool IsElementSpecified(IEnumerable<IElement> elements, string argument)
            => elements.Any(x => x.InnerHtml
                .Contains(argument));

        public string GetImageUrl(IDocument document)
            => ImageUrl
                .Match(document.Images
                    .FirstOrDefault(x => x.OuterHtml
                        .Contains("https://cdn.myanimelist.net/images/userimages/"))
                    .OuterHtml)
                .Value;

        public static bool IsMalSupporter(IDocument document)
            => document
                .All
                .First(x => x.ClassName == "link")
                .Descendents()
                .FirstOrDefault()
                .TextContent == "Supporter";

        public static double GetScore(IDocument document)
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

        public static IEnumerable<IElement> GetClearfixElements(IDocument document)
            => document
                .All
                .Where(x => x.ClassName == "clearfix");

        public static string GetLastOnline(IEnumerable<IElement> elements)
            => elements
                .FirstOrDefault()
                .Children[1]
                .TextContent;

        public static Gender GetGender(List<IElement> elements)
        {
            if (!IsElementSpecified(elements, "Gender"))
                return Gender.UNSPECIFIED;

            var gender = elements[1]
                .Children[1]
                .TextContent;

            switch (gender)
            {
                case "Non-Binary":
                    _ = Enum.TryParse(gender.Remove(gender.IndexOf('-'), 1), out Gender parsedGender);
                    return parsedGender;
                case "Female":
                    return Gender.MALE;
                case "Make":
                    return Gender.MALE;
                default:
                    return Gender.UNSPECIFIED;
            }
        }

        public static string GetBirthday(List<IElement> elements)
        {
            var index = 2;
            if (!IsElementSpecified(elements, "Gender"))
                index = 1;

            return IsElementSpecified(elements, "Birthday")
                ? elements[index]
                    .Children[1]
                    .TextContent
                : "No Birthday";
        }

        public string GetLocation(List<IElement> elements)
        {
            return IsElementSpecified(elements, "Location")
                ? elements.ToList()[^3]
                    .Children[1]
                    .TextContent
                : "No Location";
        }

        public static DateTime GetDateJoined(List<IElement> elements)
            => DateTime.Parse(elements[^2]
                .Children[1]
                .TextContent);
    }
}