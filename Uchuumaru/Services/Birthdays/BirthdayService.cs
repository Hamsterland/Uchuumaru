using System;
using System.Threading.Tasks;
using Uchuumaru.Data;

namespace Uchuumaru.Services.Birthdays
{
    /// <inheritdoc/>
    public class BirthdayService : IBirthdayService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _context;

        /// <summary>
        /// Constructs a new <see cref="BirthdayService"/>.
        /// </summary>
        public BirthdayService(UchuumaruContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task SetBirthday(ulong userId, DateTime birthday)
        {
            var users = _context
                .Users
                .Where(user => user.UserId == userId);

            foreach (var user in users)
            {
                user.Birthday = birthday;
            }

            await _context.SaveChangesAsync();
        }
    }
}