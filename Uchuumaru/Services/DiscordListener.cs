using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Uchuumaru.Services
{
    public partial class DiscordListener : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IMediator _mediator;
        private readonly CommandService _commands;

        public DiscordListener(
            DiscordSocketClient client, 
            IMediator mediator, 
            CommandService commands)
        {
            _client = client;
            _mediator = mediator;
            _commands = commands;
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
            _client.GuildMemberUpdated += GuildMemberUpdated;
            _commands.CommandExecuted += CommandExecuted;
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
            _client.GuildMemberUpdated -= GuildMemberUpdated;
            _commands.CommandExecuted -= CommandExecuted;
            return Task.CompletedTask;
        }
    }
}