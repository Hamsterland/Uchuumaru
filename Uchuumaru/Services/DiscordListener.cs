using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;
using Uchuumaru.Notifications;

namespace Uchuumaru.Services
{
    public partial class DiscordListener : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IMediator _mediator;
        private readonly CommandService _commands;
        private readonly ILogger _logger;

        public DiscordListener(
            DiscordSocketClient client, 
            IMediator mediator, 
            CommandService commands, 
            ILogger logger)
        {
            _client = client;
            _mediator = mediator;
            _commands = commands;
            _logger = logger;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += MessageReceived;
            _client.MessageDeleted += MessageDeleted;
            _client.MessageUpdated += MessageUpdated;
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.UserBanned += UserBanned;
            _client.UserUpdated += UserUpdated;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            _client.GuildMemberUpdated += GuildMemberUpdated;
            _commands.CommandExecuted += CommandExecuted;
            _client.Log += Log;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived -= MessageReceived;
            _client.MessageDeleted -= MessageDeleted;
            _client.MessageUpdated -= MessageUpdated;
            _client.JoinedGuild -= JoinedGuild;
            _client.LeftGuild -= LeftGuild;
            _client.UserBanned -= UserBanned;
            _client.UserUpdated -= UserUpdated;
            _client.UserJoined -= UserJoined;
            _client.UserLeft -= UserLeft;
            _client.GuildMemberUpdated -= GuildMemberUpdated;
            _commands.CommandExecuted -= CommandExecuted;
            _client.Log -= Log;
            return Task.CompletedTask;
        }

        private async Task Log(LogMessage log)
        {
            await _mediator.Publish(new LogMessageNotification(log));
        }
    }
}