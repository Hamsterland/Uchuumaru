using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using static Uchuumaru.MyAnimeList.Constants;

namespace Uchuumaru.Services.MyAnimeList
{
    public class ActivityService : IActivityService
    {
        private const string _rss = "rss.php?type=r{0}&u={1}";
        private const string _anime = "w";
        private const string _manga = "m";
        private const string _pubDate = "pubDate";
        
        private readonly HttpClient _client = new();
        
        public async Task<double?> CheckActivity(string username, ListOptions options)
        {
            var url = options switch
            {
                ListOptions.Anime => MyAnimeListRootUrl + string.Format(_rss, _anime, username),
                ListOptions.Manga => MyAnimeListRootUrl + string.Format(_rss, _manga, username),
                _ => throw new InvalidOperationException()
            };

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var dates = new List<DateTime>();
            var reader = new XmlTextReader(url);
            
            while (reader.Read())
            {
                if (reader.Name == _pubDate)
                {
                    reader.Read();
                    if (string.IsNullOrWhiteSpace(reader.Value))
                        continue;
                    if (DateTime.TryParse(reader.Value.Trim(), out var date))
                        dates.Add(date);
                }
            }

            dates = dates.OrderBy(x => x).ToList();
            var diff = dates.Max().Subtract(dates.Min());
            var average = TimeSpan.FromTicks(diff.Ticks / (dates.Count - 1));
            return average.TotalDays;
        }
    }
}