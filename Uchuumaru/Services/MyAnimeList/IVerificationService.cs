using System.Threading.Tasks;
using Uchuumaru.MyAnimeList.Models;

namespace Uchuumaru.Services.MyAnimeList
{
    public interface IVerificationService
    {
        Task<Profile> GetProfile(ulong userId);
        Task Begin(ulong userId, int token);
        int GetToken();
    }
}