using Discord;
using Discord.WebSocket;
using MediatR;

namespace Uchuumaru.Notifications.User
{
    /// <summary>
    /// An <see cref="INotification"/> that represents information from when
    /// a user is banned. 
    /// </summary>
    public class UserBannedNotification : INotification
    {
        /// <summary>
        /// The user that was banned.
        /// </summary>
        public IUser User { get; }
        
        /// <summary>
        /// The Guild where the ban took place.
        /// </summary>
        public SocketGuild Guild { get; }
        
        /// <summary>
        /// Constructs a new <see cref="UserBannedNotification"/>. 
        /// </summary>
        /// <param name="guild"><see cref="Guild"/></param>
        /// <param name="user"><see cref="User"/></param>
        public UserBannedNotification(IUser user, SocketGuild guild)
        {
            User = user;
            Guild = guild;
        }
    }
}