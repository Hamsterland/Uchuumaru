using System;
using System.Data.SqlTypes;
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
            _client.Ready += Ready;
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
        
        private Task Ready()
        {
            _logger.Fatal("Ready Hit");
            _client.Ready -= Ready; 
            
            Task.Run(async () =>
            {
                _logger.Fatal("Task.Run() Hit");
                var guild = _client.GetGuild(301123999000166400);
    
                _logger.Fatal($"Guild is null?: {guild is null}");
                
                var mods = guild
                    .Users
                    .Where(x => x.Roles.Any(role => role.Id == 301125242749714442))
                    .ToList();
    
                _logger.Fatal($"Mods found.");
                
                while (true)
                {
                    foreach (var mod in mods)
                    {
                        await _client.SetGameAsync($"with {mod.Nickname ?? mod.Username}");
                        _logger.Fatal($"Set game");
                        await Task.Delay(900000);
                        _logger.Fatal($"Task waiting");
                    }
                }
            });
            
            return Task.CompletedTask;
        }
    }
}