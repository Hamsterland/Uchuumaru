using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Uchuumaru.MyAnimeList.Models;
using static Uchuumaru.MyAnimeList.Constants;

namespace Uchuumaru.MyAnimeList.Parsers
{
    /// <summary>
    /// Contains methods to parse information from a MyAnimeList profile.
    /// </summary>
    public class ProfileParser : ParserBase
    {
        private readonly Regex _imageUrl =
            new("https://cdn.myanimelist.net/images/userimages/(?<userId>[0-9]+).(png|jp(e)?g|gif)");

        private const string _gender = "Gender";
        private const string _birthday = "Birthday";
        private const string _location = "Location";
        private const string _scoreLabel = "score-label score-{0}";
        private const string _link = "link";
        private const string _supporter = "Supporter";
        private const string _clearfix = "clearfix";
        private List<IElement> _clearfixElements;

        /// <summary>
        /// Downloads the document for this parser. In other words, it sets the
        /// <see cref="ParserBase.HtmlDocument"/> and other relevant fields/properties.
        /// </summary>
        /// <remarks>
        /// This method MUST be called FIRST before trying to parse anything.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Download(string username)
        {
            await DownloadDocument(MyAnimeListRootUrl + ProfileUrl + username);

            _clearfixElements = HtmlDocument
                .All
                .Where(x => x.ClassName == _clearfix)
                .ToList();
        }

        [DocumentProperty]
        public string GetImageUrl()
        {
            return _imageUrl
                .Match(base.HtmlDocument
                    .Images
                    .FirstOrDefault()
                    .OuterHtml)
                .Value;
        }

        [DocumentProperty]
        public int GetUserId()
        {
            return int.TryParse(_imageUrl
                .Match(GetImageUrl())
                .Groups["userId"]
                .Value, out var id)
                ? id
                : 0;
        }

        [DocumentProperty]
        public bool GetSupporter()
        {
            return base.HtmlDocument
                .All
                .First(x => x.ClassName == _link)
                .Descendents()
                .FirstOrDefault()
                .TextContent == _supporter;
        }

        [DocumentProperty]
        public double GetAnimeMeanScore()
        {
            var elements = base.HtmlDocument
                .All
                .ToList();

            double score = 0;

            for (var i = 10; i > 0; i--)
            {
                var label = string.Format(_scoreLabel, i);
                var node = elements.FirstOrDefault(x => x.ClassName == label);

                if (node is null)
                    continue;

                if (elements.HasClass(label))
                    score = double.Parse(elements
                        .FirstOrDefault(x => x.ClassName == label)
                        .TextContent);
            }

            return score;
        }

        [ClearfixProperty]
        public string GetLastOnline()
        {
            return _clearfixElements
                .FirstOrDefault()?
                .Children[1]
                .TextContent;
        }


        [ClearfixProperty]
        public Gender GetGender()
        {
            if (!IsElementSpecified(_gender))
                return Gender.Unspecified;

            var gender = _clearfixElements[1]
                .Children[1]
                .TextContent;

            return gender switch
            {
                "Non-Binary" => Gender.NonBinary,
                "Female" => Gender.Female,
                "Male" => Gender.Male,
                _ => Gender.Unspecified
            };
        }

        [ClearfixProperty]
        public string GetBirthday()
        {
            if (!IsElementSpecified(_birthday))
                return null;

            var index = 2;
            if (!IsElementSpecified(_gender))
                index = 1;

            return _clearfixElements[index]
                .Children[1]
                .TextContent;
        }
        
        [ClearfixProperty]
        public string GetLocation()
        {
            return IsElementSpecified(_location)
                ? _clearfixElements[^3]
                    .Children[1]
                    .TextContent
                : null;
        }

        [ClearfixProperty]
        public DateTime GetDateJoined()
        {
            return DateTime.Parse(_clearfixElements[^2]
                .Children[1]
                .TextContent);
        }
        
        private bool IsElementSpecified(string name)
        {
            return _clearfixElements
                .Any(x => x.InnerHtml
                    .Contains(name));
        }
    }
}