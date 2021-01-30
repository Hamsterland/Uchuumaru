using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Uchuumaru.Services.Infractions.Mutes;

namespace Uchuumaru.Services
{
    public class Startup : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMuteService _mute;
        private readonly IHostApplicationLifetime _lifetime;

        public Startup(
            DiscordSocketClient client, 
            IConfiguration configuration,
            ILogger logger,
            IMuteService mute, IHostApplicationLifetime lifetime)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
            _mute = mute;
            _lifetime = lifetime;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _configuration["Discord:Token"];

            if (string.IsNullOrEmpty(token))
            {
                _logger.Fatal("{Message}", "The bot token was not found.");
                return; 
            }

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                await _mute.CacheMutes();
            }
            catch (Exception ex)
            {
                _logger.Fatal("{Message}", ex.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
    }
}