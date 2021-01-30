using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Serilog;
using Serilog.Events;
using Uchuumaru.Notifications;

namespace Uchuumaru.Listeners
{
    public class LogMessageListener : INotificationHandler<LogMessageNotification>
    {
        private readonly ILogger _logger;

        public LogMessageListener(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(LogMessageNotification notification, CancellationToken cancellationToken)
        {
            var level = ConvertToLogEventLevel(notification.LogMessage.Severity);
            var exception = notification.LogMessage.Exception;
            var message = notification.LogMessage.Message;
            _logger.Write(level, exception, "{Message}", message);
            return Task.CompletedTask;
        }

        private static LogEventLevel ConvertToLogEventLevel(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Debug => LogEventLevel.Debug,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Critical => LogEventLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }
    }
}