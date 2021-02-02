using System;
using System.Threading.Tasks;

namespace Uchuumaru.Services.Birthdays
{
    /// <summary>
    /// Describes a service that handles birthday information for users.
    /// </summary>
    public interface IBirthdayService
    {
        /// <summary>
        /// Sets the birthday for a user across all Guilds.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="birthday">The birthday date.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task SetBirthday(ulong userId, DateTime birthday);
    }
}