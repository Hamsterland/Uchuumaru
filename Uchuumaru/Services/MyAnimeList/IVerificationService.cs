using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Uchuumaru.MyAnimeList.Models;

namespace Uchuumaru.Services.MyAnimeList
{
    public interface IVerificationService
    {
        Task<Profile> GetProfile(ulong userId);
        Task<bool> AccountExists(string username);
        Task<VerificationResult> Authenticate(string username);
        Task<VerificationResult> Verify(IGuildUser author, string username, int token);
        int GetToken();
    }
}