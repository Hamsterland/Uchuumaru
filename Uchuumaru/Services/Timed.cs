using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ILogger = Serilog.ILogger;


namespace Uchuumaru.Services
{
    internal class AutoRestartService : IHostedService
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public AutoRestartService(ILogger logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Fatal("Application AutoRestart is starting");

            _timer = new Timer(async _ =>
                {
                    Process.Start("Uchuumaru");
                    Environment.Exit(0);
                }, null, TimeSpan.FromHours(1), 
                Timeout.InfiniteTimeSpan);

            return Task.CompletedTask;
        }
        

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Fatal("Application AutoRestart is stopping");
            return Task.CompletedTask;
        }
    }
}