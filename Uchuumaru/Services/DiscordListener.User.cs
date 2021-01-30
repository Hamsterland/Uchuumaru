using System.Threading.Tasks;
using Discord.WebSocket;
using Uchuumaru.Notifications.Users;

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
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            await _mediator.Publish(new UserBannedNotification(user, guild)); 
        }

        /// <summary>
        /// This method is called on the <see cref="UserUpdated"/> event and
        /// publishes a <see cref="UserUpdatedNotification"/>.
        /// </summary>
        /// <param name="before">The user before update.</param>
        /// <param name="after">The user after update.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task UserUpdated(SocketUser before, SocketUser after)
        {
            await _mediator.Publish(new UserUpdatedNotification(before, after));
        }

        /// <summary>
        /// This method is called on the <see cref="GuildMemberUpdated"/> event and
        /// publishes a <see cref="GuildMemberUpdatedNotification"/>.
        /// </summary>
        /// <param name="before">The user before update.</param>
        /// <param name="after">The user after update.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
            await _mediator.Publish(new GuildMemberUpdatedNotification(before, after));
        }
        
        /// <summary>
        /// This method is called on the <see cref="UserJoined"/> event and publishes
        /// a <see cref="UserJoined"/> notification. 
        /// </summary>
        /// <param name="user">The user that joined.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task UserJoined(SocketGuildUser user)
        {
            await _mediator.Publish(new UserJoinedNotification(user));
        }
        
        /// <summary>
        /// This method is called on the <see cref="UserLeft"/> event and publishes
        /// a <see cref="UserLeft"/> notification. 
        /// </summary>
        /// <param name="user">The user that left.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task UserLeft(SocketGuildUser user)
        {
            await _mediator.Publish(new UserLeftNotification(user));
        }
    }
}