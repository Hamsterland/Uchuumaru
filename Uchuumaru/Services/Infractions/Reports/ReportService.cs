using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Services.Infractions.Reports
{
    /// <inheritdoc/>
    public class ReportService : IReportService
    {
        /// <summary>
        /// The application database context.
        /// </summary>
        private readonly UchuumaruContext _context;

        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="ReportService"/> with the given
        /// injected dependencies.
        /// </summary>
        public ReportService(UchuumaruContext context, DiscordSocketClient client)
        {
            _context = context;
            _client = client;
        }

        /// <inheritdoc/>
        public async Task Report(ulong userId, ulong channelId, ulong guildId, string report)
        {
            var guild = await _context
                .Guilds
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var socketGuild = _client.GetGuild(guildId);
            var user = socketGuild.GetUser(userId);
            var channel = socketGuild.GetChannel(channelId);
            var reportChannel = socketGuild.GetChannel(guild.ReportChannelId) as ITextChannel;

            if (reportChannel is null)
            {
                return; 
            }

            var moderatorRole = socketGuild.GetRole(guild.ModeratorRoleId);
            
            var embed = new EmbedBuilder()
                .WithColor(Constants.DefaultColour)
                .WithAuthor(author => author
                    .WithName($"{user} sent a report")
                    .WithIconUrl(user.GetAvatarUrl()))
                .AddField("Channel", $"{channel.Name} ({channel.Id})")
                .AddField("Report", report)
                .Build();

            if (moderatorRole is not null)
            {
                await reportChannel.SendMessageAsync(moderatorRole.Mention, embed: embed);
            }
            else
            {
                await reportChannel.SendMessageAsync(embed: embed);
            }
        }
    }
}