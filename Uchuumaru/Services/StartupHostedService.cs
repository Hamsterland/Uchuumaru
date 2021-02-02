using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Uchuumaru.Services.Infractions.Mutes;

namespace Uchuumaru.Services
{
    public class StartupHostedService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMuteService _mute;
        
        public StartupHostedService(
            DiscordSocketClient client, 
            IConfiguration configuration,
            ILogger logger,
            IMuteService mute)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
            _mute = mute;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _configuration["Discord:Token"];

            if (string.IsNullOrEmpty(token))
            {
                _logger.Fatal("The bot token was not found.");
                return; 
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _mute.CacheMutes();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
        
        public static void Initiate()
        {
            Console.WriteLine("EWRUFHGPQWEURGPQIEWURGPIEURBGPWEUIRBGPUEIRBGPUESRBGPOIUSERBFGOISUERGBSEHRBG");
            Process.Start("Uchuumaru.exe");
            Environment.Exit(0);
        }
    }
}