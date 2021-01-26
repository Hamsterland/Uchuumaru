using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Users
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="UchuumaruContext"/>.
        /// </summary>
        public UserService(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        /// <inheritdoc/>
        public async Task<UserSummary> GetUserSummary(ulong guildId, ulong userId)
        {
            var user = await _uchuumaruContext
                .Users
                .Where(x => x.UserId == userId)
                .Where(x => x.Guild.GuildId == guildId)
                .OrderBy(x => x)
                .Include(x => x.Usernames)
                .Include(x => x.Nicknames)
                .AsSplitQuery()
                .FirstOrDefaultAsync();
            
            return user?.ToUserSummary();
        }
    }
}