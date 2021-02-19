using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;
using Uchuumaru.Notifications;
using Uchuumaru.Utilities;

namespace Uchuumaru.Services.Messages
{
    public class MessageDeletedListener : INotificationHandler<MessageDeletedNotification>
    {
        private readonly UchuumaruContext _uchuumaruContext;

        private readonly DiscordSocketClient _client;

        public MessageDeletedListener(
            UchuumaruContext uchuumaruContext, 
            DiscordSocketClient client)
        {
            _uchuumaruContext = uchuumaruContext;
            _client = client;
        }
        
        public async Task Handle(MessageDeletedNotification notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;
            var channel = notification.Channel;

            if (!message.HasValue)
                return;
            
            var value = message.Value;
            
            if (value.Author.IsBot)
                return;
            
            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == (channel as IGuildChannel).GuildId, cancellationToken);

            if (guild is null)
                throw new EntityNotFoundException<Guild>();
            
            if (guild.MessageChannelId == 0)
                return;
            
            if (_client.GetChannel(guild.MessageChannelId) is not ITextChannel textChannel)
                return;

            var embed = new EmbedBuilder()
                .WithTitle("Message Deleted")
                .WithColor(Constants.DefaultColour)
                .AddField("Author", value.Author.Represent())
                .AddField("Channel", channel.Represent());

            if (!string.IsNullOrEmpty(value.Content))
                embed.AddField("Content", value.Content);
            
            if (value.IsPinned)
                embed.AddField("Pinned", true);
            
            if (value.Reactions.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var (emote, metadata) in value.Reactions)
                    builder.AppendLine($"{metadata.ReactionCount}x {emote.Name}");

                embed.AddField("Reactions", builder);
            }

            if (value.Attachments.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var att in value.Attachments)
                    builder.AppendLine($"{att.Filename} ({att.Size}, {att.Width}x{att.Height})");

                embed.AddField("Attachments", builder);
            }

            await textChannel.SendMessageAsync(embed: embed.Build());
        }
    }
}
