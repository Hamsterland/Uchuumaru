using System.Threading.Tasks;

namespace Uchuumaru.Services.MyAnimeList
{
    public interface IActivityService
    {
        Task<double?> CheckActivity(string username, ListOptions options);
    }
}