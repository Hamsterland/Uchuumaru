using System.Threading.Tasks;

namespace Uchuumaru.Services.Infractions.Bans
{
    public interface IBanService
    {
        Task Unban(ulong guildId, ulong subjectId, ulong moderatorId);
    }
}