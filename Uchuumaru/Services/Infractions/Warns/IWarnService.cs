using System.Threading.Tasks;

namespace Uchuumaru.Services.Infractions.Warns
{
    public interface IWarnService
    {
        Task CreateWarn(
            ulong guildId,
            ulong subjectId,
            ulong moderatorId,
            string reason = null);
    }
}