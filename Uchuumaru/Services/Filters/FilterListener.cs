using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Uchuumaru.Notifications.Message;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// An <see cref="INotificationHandler{TNotification}"/> that listens to the <see cref="MessageReceivedNotification"/>.
    /// </summary>
    public class FilterListener : INotificationHandler<MessageReceivedNotification>
    {
        /// <summary>
        /// The filter service.
        /// </summary>
        private readonly IFilterService _filter;

        /// <summary>
        /// Constructs a new <see cref="FilterListener"/> with the given
        /// injected dependencies.
        /// </summary>
        public FilterListener(IFilterService filter)
        {
            _filter = filter;
        }

        /// <summary>
        /// Determines if the content of the received <paramref name="notification.Message"/> violates the filter.
        /// </summary>
        /// <param name="notification">The received notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;
            var guild = (message.Channel as IGuildChannel).Guild;
            
            var status = await _filter.GetFilterStatus(guild.Id);
            var expressions = status.Expressions;

            var regexes = expressions
                .Select(expression => new Regex(expression))
                .ToList();

            if (regexes.Any(regex => regex.IsMatch(message.Content)))
            {
                await message.DeleteAsync(); 
            }
        }
    }
}