using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using ILogger = Serilog.ILogger;


namespace Uchuumaru.Services
{
    internal class TimedHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService(ILogger logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Fatal("Timed Background Service is starting");

            _timer = new Timer(async _ =>
                {
                    Process.Start("Uchuumaru.exe");
                    Environment.Exit(0);
                }, null, TimeSpan.FromSeconds(5), 
                Timeout.InfiniteTimeSpan);

            return Task.CompletedTask;
        }
        

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Fatal("Timed Background Service is stopping");
            return Task.CompletedTask;
        }
    }
}