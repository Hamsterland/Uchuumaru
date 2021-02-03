
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.Infractions.Reports;

namespace Uchuumaru.Modules
{
    [Name("Report")]
    [Group("report")]
    [Summary("When pinging the moderator role directly is too bait.")]
    public class ReportModule : ModuleBase<SocketCommandContext>
    {
        private readonly IReportService _report;

        public ReportModule(IReportService report)
        {
            _report = report;
        }

        [Command]
        [Summary("Sends a report to the moderators.")]
        public async Task Report([Remainder] string report)
        {
            await _report.Report(Context.User.Id, Context.Channel.Id, Context.Guild.Id, report);
            await Context.Message.DeleteAsync();
            await Context.User.SendMessageAsync("Your report has been sent. Thank you.");
        }
    }
}