using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Serilog;
using Uchuumaru.Modules;
using Uchuumaru.Notifications;
using Uchuumaru.Notifications.Commands;
using Uchuumaru.Services.MyAnimeList;

namespace Uchuumaru.Services.Commands
{
    /// <summary>
    /// A <see cref="NotificationHandler{TNotification}"/> that listens to the <see cref="CommandExecutedNotification"/>
    /// to handle post- command execution responses.
    /// </summary>
    public class CommandExecutedHandler : INotificationHandler<CommandExecutedNotification>
    {
        private readonly DiscordSocketClient _client;
        private readonly IVerificationService _verification;

        public CommandExecutedHandler(DiscordSocketClient client, IVerificationService verification)
        {
            _client = client;
            _verification = verification;
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

            if (!result.IsSuccess && result is ExecuteResult executeResult)
            {
                // TEMPORARY LOGGING
                var me = _client.GetUser(330746772378877954);

                var message = new StringBuilder()
                    .AppendLine(Format.Bold("ERROR REASON"))
                    .AppendLine(result.ErrorReason)
                    .AppendLine()
                    .AppendLine(Format.Bold("COMMAND ERROR"))
                    .AppendLine(result.Error?.ToString())
                    .AppendLine()
                    .AppendLine(Format.Bold("EXCEPTION"))
                    .AppendLine(executeResult.Exception.Message)
                    .AppendLine(executeResult.Exception.Source)
                    .AppendLine(executeResult.Exception.HelpLink)
                    .AppendLine(executeResult.Exception.StackTrace)
                    .ToString();

                if (message.Length > 2000)
                {
                    // It's fine if we lose a couple characters
                    var message1 = message.Substring(0, message.Length / 2);
                    var message2 = message.Substring(message.Length / 2, message.Length - 1);
                    await me.SendMessageAsync(message1);
                    await me.SendMessageAsync(message2);
                }
                else
                {
                    await me.SendMessageAsync(message);
                }

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