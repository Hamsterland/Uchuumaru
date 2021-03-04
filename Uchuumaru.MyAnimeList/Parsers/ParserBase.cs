using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Uchuumaru.MyAnimeList.Parsers
{
    /// <summary>
    /// A base class that represents a parser's functionalities.
    /// </summary>
    public class ParserBase
    {
        /// <summary>
        /// The Http client.
        /// </summary>
        public HttpClient Httpclient { get; } = new();
        
        /// <summary>
        /// The Html parser.
        /// </summary>
        public HtmlParser HtmlParser { get; } = new();
        
        /// <summary>
        /// The Html document being parsed.
        /// </summary>
        protected IHtmlDocument HtmlDocument { get; set; }
        
        protected async Task DownloadDocument(string url)
        {
            var html = await Httpclient.GetStringAsync(url);
            HtmlDocument = await HtmlParser.ParseDocumentAsync(html);
        }
    }
}