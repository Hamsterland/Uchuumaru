using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Uchuumaru.Data;

namespace Uchuumaru.Services.Birthdays
{
    /// <summary>
    /// An <see cref="IHostedService"/> responsible for tracking user birthdays.
    /// </summary>
    public class BirthdayHostedService : IHostedService
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
        /// Constructs a new <see cref="BirthdayHostedService"/> with the given
        /// injected dependencies.
        /// </summary>
        public BirthdayHostedService(UchuumaruContext context, DiscordSocketClient client)
        {
            _context = context;
            _client = client;
        }

        private Timer _timer;
        private const string _title = "Tanjoubi Omedetou!";
        private const string _message = "It is {0}'s Birthday!";
        private const string _url = "https://image.myanimelist.net/ui/BQM6jEZ-UJLgGUuvrNkYUCG8p-X1WhZLiR4h-oxkqQd0AETF23XSKPPjpo3qG3m8UUKiCNQTx80pWGq9ym3lQw";

        /// <summary>
        /// Initializes the private <see cref="_timer"/> that executes immediately and subsequently
        /// every 24 hours. 
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion,
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                async _ => await ExecuteBirthday(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(24)
            );

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends a birthday message to the birthday channel in all Guilds where
        /// a user has a birthday.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        private async Task ExecuteBirthday()
        {
            var users = _context
                .Users
                .ToList();

            // Each List<User> share the same Guild.
            var birthdayGroups = users
                .Where(x => x.Birthday.Day.Equals(DateTime.UtcNow.Day) 
                            && x.Birthday.Month.Equals(DateTime.UtcNow.Month))
                .GroupBy(user => user.Guild.Id)
                .Select(group => group.ToList())
                .ToList();

            foreach (var group in birthdayGroups)
            {
                var guild = await _context
                    .Guilds
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.GuildId == group[0].Guild.GuildId);

                foreach (var user in group)
                {
                    var socketGuild = _client.GetGuild(user.Guild.GuildId);
                    var socketUser = socketGuild.GetUser(user.UserId);
                    var birthdayId = guild.BirthdayChannelId;

                    if (birthdayId == 0)
                        return;

                    var birthdayChannel = socketGuild.GetChannel(birthdayId) as ITextChannel;

                    if (birthdayChannel is null)
                        return;

                    var embed = new EmbedBuilder()
                        .WithTitle(_title)
                        .WithDescription(string.Format(_message, socketUser))
                        .WithImageUrl(_url)
                        .WithColor(Constants.DefaultColour)
                        .Build();

                    await birthdayChannel.SendMessageAsync(embed: embed);
                }
            }
        }

        /// <summary>
        /// Disposes the private <see cref="_timer"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}