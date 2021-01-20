using System.Threading.Tasks;
using Discord.WebSocket;
using Uchuumaru.Notifications.User;

namespace Uchuumaru.Services
{
    /// <summary>
    /// Events related to users.
    /// </summary>
    public partial class DiscordListener
    {
        /// <summary>
        /// This method is called on the <see cref="UserBanned"/> event and
        /// publishes a <see cref="UserBannedNotification"/>.
        /// </summary>
        /// <param name="user">The banned user.</param>
        /// <param name="guild">The guild of the ban.</param>
        /// <returns></returns>
        private async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            await _mediator.Publish(new UserBannedNotification(user, guild)); 
        }
    }
}