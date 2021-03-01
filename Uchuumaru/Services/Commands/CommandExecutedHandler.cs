using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Serilog;
using Uchuumaru.Notifications;
using Uchuumaru.Notifications.Commands;

namespace Uchuumaru.Services.Commands
{
    /// <summary>
    /// A <see cref="NotificationHandler{TNotification}"/> that listens to the <see cref="CommandExecutedNotification"/>
    /// to handle post- command execution responses.
    /// </summary>
    public class CommandExecutedHandler : INotificationHandler<CommandExecutedNotification>
    {
        private readonly DiscordSocketClient _client;

        public CommandExecutedHandler(DiscordSocketClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Determines if the command execution was successful or not. If it was not successful, the error
        /// reason is logged.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion. 
        /// </returns>
        public async Task Handle(CommandExecutedNotification notification, CancellationToken cancellationToken)
        {
            var (command, contex, result) = notification.Deconstruct();

            if (!result.IsSuccess)
            {
                // TEMPORARY LOGGING
                var me = _client.GetUser(330746772378877954);

                var message = new StringBuilder()
                    .AppendLine(Format.Bold("ERROR REASON"))
                    .AppendLine(result.ErrorReason)
                    .AppendLine()
                    .AppendLine(Format.Bold("COMMAND ERROR"))
                    .AppendLine(result.Error?.ToString())
                    .ToString();

                await me.SendMessageAsync(message);

                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                    case CommandError.UnmetPrecondition:
                        return;
                }

                await contex.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}