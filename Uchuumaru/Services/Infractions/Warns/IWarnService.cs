using System.Threading.Tasks;

namespace Uchuumaru.Services.Infractions.Warns
{
    public interface IWarnService
    {
        Task Create(ulong guildId,ulong subjectId, ulong moderatorId, bool log, string reason = null);
        Task Rescind(int id, ulong guildId);
        Task Delete(int id, ulong guildId);
    }
}