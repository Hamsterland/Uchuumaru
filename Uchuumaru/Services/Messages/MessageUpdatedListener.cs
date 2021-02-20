using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;
using Uchuumaru.Notifications;
using Uchuumaru.Utilities;

namespace Uchuumaru.Services.Messages
{
    public class MessageUpdatedListener : INotificationHandler<MessageUpdatedNotification>
    {
        private readonly UchuumaruContext _uchuumaruContext;
        private readonly DiscordSocketClient _client;

        public MessageUpdatedListener(
            UchuumaruContext uchuumaruContext,
            DiscordSocketClient client)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
        }

        public async Task Handle(MessageUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var (before, after channel) = notification.Deconstruct();

            if (!before.HasValue)
                return;

            var guild = await _uchuumaruContext
                .Guilds
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuildId == (channel as IGuildChannel).GuildId, cancellationToken);

            if (guild is null)
                throw new EntityNotFoundException<Guild>();

            if (guild.MessageChannelId == 0)
                return;

            if (_client.GetChannel(guild.MessageChannelId) is not IMessageChannel messageChannel)
                return;

            var beforeValue = before.Value;

            if (beforeValue.Author.IsBot)
                return;

            if (beforeValue.Content == after.Content && beforeValue.IsPinned == after.IsPinned)
                return;

            var embed = new EmbedBuilder()
                .WithTitle("Message Updated")
                .WithColor(Constants.DefaultColour)
                .AddField("Author", after.Author.Represent())
                .AddField("Channel", channel.Represent());

            if (beforeValue.Content != after.Content)
            {
                string beforeContent = null;
                string afterContent = null;

                const int fieldLimit = 1024; 
                const int characterLimit = 1021;

                // If either of the edited message versions are larger than the field limit, truncate them.
                // Truncate() appends "..." to the end of the string.
                if (beforeValue.Content.Length > fieldLimit || after.Content.Length > fieldLimit)
                {
                    if (beforeValue.Content.Length > fieldLimit)
                        beforeContent = beforeValue.Content.Truncate(characterLimit);

                    if (after.Content.Length > fieldLimit)
                        afterContent = after.Content.Truncate(characterLimit);
                    
                    embed.WithFooter("The message is too long to display and was truncated.");
                }
                else
                {
                    beforeContent = beforeValue.Content;
                    afterContent = after.Content;
                }
                
                embed
                    .AddField("Before", beforeContent)
                    .AddField("After", afterContent);
            }

            if (beforeValue.IsPinned != after.IsPinned)
                embed.AddField("Pinned", $"{beforeValue.IsPinned} to {after.IsPinned}");

            await messageChannel.SendMessageAsync(embed: embed.Build());
        }
    }
}