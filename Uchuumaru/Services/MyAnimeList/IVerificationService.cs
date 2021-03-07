using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Uchuumaru.MyAnimeList.Models;

namespace Uchuumaru.Services.MyAnimeList
{
    public interface IVerificationService
    {
        Task<Profile> GetProfile(ulong userId);
        Task Begin(IGuildUser author, string username, ITextChannel channel);
    }
}