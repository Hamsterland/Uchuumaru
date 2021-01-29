using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
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
        /// <summary>
        /// The application logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructs a new <see cref="CommandExecutedHandler"/> with the given injected
        /// dependencies.
        /// </summary>
        /// <param name="logger"></param>
        public CommandExecutedHandler(ILogger logger)
        {
            _logger = logger;
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