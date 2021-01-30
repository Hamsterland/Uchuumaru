using Discord;
using MediatR;

namespace Uchuumaru.Notifications
{
    public class LogMessageNotification : INotification
    {
        public LogMessage LogMessage { get; }

        public LogMessageNotification(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }
}