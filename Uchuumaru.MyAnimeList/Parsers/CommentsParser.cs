using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Uchuumaru.MyAnimeList.Constants;

namespace Uchuumaru.MyAnimeList.Parsers
{
    public class CommentsParser : ParserBase
    {
        private readonly Regex _username = new("(?<Username>(.?)+)'s Comments");
        private const string _contentWrapper = "contentWrapper";
        
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
        public async Task Download(int id)
        {
            await DownloadDocument(MyAnimeListRootUrl + string.Format(CommentsUrl, id));
        }

        [DocumentProperty]
        public string GetUsername()
        {
            return _username
                .Matches(HtmlDocument
                    .GetElementById(_contentWrapper)
                    .FirstChild
                    .TextContent)
                .FirstOrDefault()
                .Groups["Username"]
                .Value;
        }
    }
}