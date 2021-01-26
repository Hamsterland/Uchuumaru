using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Notifications.Guilds;

namespace Uchuumaru.Services.Guilds
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> for the <see cref="JoinedGuildNotification"/>.
    /// </summary>
    public class JoinedGuildListener : INotificationHandler<JoinedGuildNotification>
    {
        /// <summary>
        /// The guild service.
        /// </summary>
        private readonly IGuildService _guild;

        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _uchuumaruContext;

        /// <summary>
        /// Constructs a new <see cref="JoinedGuildListener"/> with the
        /// given injected dependencies.
        /// </summary>
        public JoinedGuildListener(IGuildService guild, UchuumaruContext uchuumaruContext)
        {
            _guild = guild;
            _uchuumaruContext = uchuumaruContext;
        }

        /// <summary>
        /// Adds the guild that was just joined to the database.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(JoinedGuildNotification notification, CancellationToken cancellationToken)
        {
            await _guild.AddGuild(notification.Guild.Id);
            await CreateMuteRole(notification.Guild);
        }

        private async Task CreateMuteRole(SocketGuild socketGuild)
        {
            const string name = "kb-mute";
            var role = await socketGuild.CreateRoleAsync(name, null, null, false, RequestOptions.Default);

            foreach (var channel in socketGuild.Channels)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (channel is ITextChannel textChannel)
                {
                    await textChannel.AddPermissionOverwriteAsync(role, new OverwritePermissions(
                        sendMessages: PermValue.Deny,
                        addReactions: PermValue.Deny,
                        embedLinks: PermValue.Deny,
                        attachFiles: PermValue.Deny,
                        useExternalEmojis: PermValue.Deny,
                        useVoiceActivation: PermValue.Deny
                    ));   
                }
                
                if (channel is IVoiceChannel voiceChannel)
                {
                    await voiceChannel.AddPermissionOverwriteAsync(role, new OverwritePermissions(speak: PermValue.Deny));    
                }
                
            }

            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == socketGuild.Id);

            guild.MuteRoleId = role.Id;
            await _uchuumaruContext.SaveChangesAsync();
        }
    }
}