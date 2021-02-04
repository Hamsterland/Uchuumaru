// Feature on-hold.


// using System.Threading;
// using System.Threading.Tasks;
// using Discord;
// using MediatR;
// using Uchuumaru.Notifications.Message;
// using Uchuumaru.Services.Infractions.Reports;
//
// namespace Uchuumaru.Services.Reports
// {
//     public class ReportListener : INotificationHandler<MessageReceivedNotification>
//     {
//         private readonly IReportService _report;
//
//         public ReportListener(IReportService report)
//         {
//             _report = report;
//         }
//
//         public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
//         {
//             var message = notification.Message;
//             var channel = message.Channel;
//             var author = message.Author;
//
//             if (channel is not IDMChannel dmChannel)
//             {
//                 return;
//             }
//
//             if (!message.Content.StartsWith("+report"))
//             {
//                 return;
//             }
//
//             var report = message.Content.Replace("+report", string.Empty);
//             // await _report.Report(author.Id, channel.Id, 0, ))
//         }
//     }
// }