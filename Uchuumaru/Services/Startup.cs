using System;
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

        public Startup(
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

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                await _mute.CacheMutes();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.Message);
            }
            
            var timer = new Timer(
                async  =>
                {
                    Environment.Exit(0);
                },  
                null, 
                TimeSpan.Zero, 
                TimeSpan.FromSeconds(30));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
    }
}