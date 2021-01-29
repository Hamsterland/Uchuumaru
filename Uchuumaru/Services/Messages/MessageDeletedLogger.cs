using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Notifications;

namespace Uchuumaru.Services.Messages
{
    public class MessageDeletedLogger : INotificationHandler<MessageDeletedNotification>
    {
        private readonly UchuumaruContext _uchuumaruContext;

        public MessageDeletedLogger(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        public async Task Handle(MessageDeletedNotification notification, CancellationToken cancellationToken)
        {
            var deletedMessage = notification.Message;
            var sourceChannel = notification.Channel;

            var sourceGuild = (sourceChannel as IGuildChannel).Guild;
            var sourceGuildId = sourceGuild.Id;
            
            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == sourceGuildId, cancellationToken);

            var messageLogChannelId = guild.MessageChannelId;

            if (messageLogChannelId == 0 || !deletedMessage.HasValue)
            {
                return;
            }
            
            
            var content = deletedMessage.Value.Content;
            var author = deletedMessage.Value.Author;
            var attachments = deletedMessage.Value.Attachments;
            var reactions = deletedMessage.Value.Reactions;
            var pinned = deletedMessage.Value.IsPinned;
            var id = deletedMessage.Value.Id;
            
            var embed = new EmbedBuilder()
                .WithTitle("Message Deleted")
                .AddField("Id", id)
                .AddMessageAuthor(author)
                .WithColor(Constants.DefaultColour);
            
            if (!string.IsNullOrEmpty(content))
            {
                embed.AddContent(content);
            }

            embed.AddChannel(sourceChannel);
            
            if (attachments.Count > 0)
            {
                embed.AddAttachments(attachments);
            }

            if (reactions.Count > 0)
            {
                embed.AddReactions(reactions);
            }

            if (pinned)
            {
                embed.AddField("Pinned", true);
            }
                    
            var messageLogChannel = await sourceGuild.GetChannelAsync(messageLogChannelId) as IMessageChannel;
            await messageLogChannel.SendMessageAsync(embed: embed.Build());
        }
    }
    
    public static class MessageDeletedExtensions
    {
        public static EmbedBuilder AddContent(this EmbedBuilder builder, string content)
        {
            return builder.AddField("Content", Format.Sanitize(content));
        }

        public static EmbedBuilder AddMessageAuthor(this EmbedBuilder builder, IUser user)
        {
            return builder.AddField("Author", $"{user} ({user.Id})");
        }
        
        public static EmbedBuilder AddAttachments(this EmbedBuilder builder, IEnumerable<IAttachment> attachments)
        {
            var sb = new StringBuilder();

            foreach (var attachment in attachments)
            {
                sb.AppendLine($"• {Format.Bold(attachment.Filename)}");
                sb.Append($"{attachment.Size}px {attachment.Width}x{attachment.Height} (Spoiler: {attachment.IsSpoiler()})");
            }

            return builder.AddField("Attachments", sb.ToString());
        }
        
        public static EmbedBuilder AddChannel(this EmbedBuilder builder, IChannel channel)
        {
            return builder.AddField("Channel", $"{channel.Name} ({channel.Id})");
        }
        
        public static EmbedBuilder AddReactions(this EmbedBuilder builder, IReadOnlyDictionary<IEmote, ReactionMetadata> reactions)
        {
            var sb = new StringBuilder();
            
            // ReSharper disable once UseDeconstruction
            foreach (var reaction in reactions)
            {
                sb.Append($"({reaction.Key.Name}, {reaction.Value.ReactionCount}), ");
            }

            return builder.AddField("Reactions", sb.ToString());
        }
    }
}