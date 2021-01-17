using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Configuration;
using Uchuumaru.Notifications;
using Uchuumaru.Notifications.Message;

namespace Uchuumaru.Services.Commands
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the MessageReceived event
    /// to handle command execution.
    /// </summary>
    public class CommandHandler : INotificationHandler<MessageReceivedNotification>
    {
        /// <summary>
        /// The client instance that runs the bot.
        /// </summary>
        private readonly DiscordSocketClient _client;
        
        /// <summary>
        /// The command service associated.
        /// </summary>
        private readonly CommandService _commands;
        
        /// <summary>
        /// The application configuration root.
        /// </summary>
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider _services;

        /// <summary>
        /// Constructs a new <see cref="CommandHandler"/> with the given injected dependencies.
        /// </summary>
        public CommandHandler(DiscordSocketClient client, CommandService commands, IConfiguration configuration, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _configuration = configuration;
            _services = services;
        }

        /// <summary>
        /// Determines whether the received message is a command. If such is true, the specified
        /// command is executed.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
        {
            if (!(notification.Message is SocketUserMessage { Author: IGuildUser user } message) || user.IsBot)
            {
                return;
            }
            
            var argPos = 0;
            var prefix = _configuration["Discord:Prefix"];
            
            if (message.HasStringPrefix(prefix, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}