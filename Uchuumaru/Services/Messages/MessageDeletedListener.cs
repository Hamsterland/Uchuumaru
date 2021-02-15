using System.Collections.Generic;
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

            if (_client.GetChannel(guild.MessageChannelId) is not ITextChannel messageChannel)
                return;
            
            var embed = new EmbedBuilder()
                .WithTitle("Message Deleted")
                .WithColor(Constants.DefaultColour)
                .AddField("Author", value.Author.Represent())
                .AddField("Channel", channel.Represent());

            if (string.IsNullOrEmpty(value.Content))
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

            await messageChannel.SendMessageAsync(embed: embed.Build());
        }

        //     public async Task Handle(MessageDeletedNotification notification, CancellationToken cancellationToken)
        //     {
        //         var deletedMessage = notification.Message;
        //         var sourceChannel = notification.Channel;
        //         var sourceGuild = (sourceChannel as IGuildChannel).Guild;
        //         var sourceGuildId = sourceGuild.Id;
        //
        //         var guild = await _uchuumaruContext
        //             .Guilds
        //             .FirstOrDefaultAsync(x => x.GuildId == sourceGuildId, cancellationToken);
        //
        //         var messageLogChannelId = guild.MessageChannelId;
        //
        //         if (messageLogChannelId == 0)
        //         {
        //             return;
        //         }
        //
        //         if (!deletedMessage.HasValue)
        //         {
        //             return;
        //         }
        //
        //         var content = deletedMessage.Value.Content;
        //         var author = deletedMessage.Value.Author;
        //         var attachments = deletedMessage.Value.Attachments;
        //         var reactions = deletedMessage.Value.Reactions;
        //         var pinned = deletedMessage.Value.IsPinned;
        //
        //         if (author.IsBot)
        //         {
        //             return;
        //         }
        //         
        //         var embed = new EmbedBuilder()
        //             .WithTitle("Message Deleted")
        //             .AddChannel(sourceChannel)
        //             .AddMessageAuthor(author)
        //             .WithColor(Constants.DefaultColour);
        //
        //         if (!string.IsNullOrEmpty(content))
        //         {
        //             embed.AddContent(content);
        //         }
        //
        //         if (attachments.Count > 0)
        //         {
        //             embed.AddAttachments(attachments);
        //         }
        //
        //         if (reactions.Count > 0)
        //         {
        //             embed.AddReactions(reactions);
        //         }
        //
        //         if (pinned)
        //         {
        //             embed.AddField("Pinned", true);
        //         }
        //
        //         var messageLogChannel = await sourceGuild.GetChannelAsync(messageLogChannelId) as IMessageChannel;
        //         await messageLogChannel.SendMessageAsync(embed: embed.Build());
        //     }
        // }
        //
        // public static class MessageDeletedExtensions
        // {
        //     public static EmbedBuilder AddContent(this EmbedBuilder builder, string content)
        //     {
        //         return builder.AddField("Content", Format.Sanitize(content));
        //     }
        //
        //     public static EmbedBuilder AddMessageAuthor(this EmbedBuilder builder, IUser user)
        //     {
        //         return builder.AddField("Author", $"{user} ({user.Id})");
        //     }
        //
        //     public static EmbedBuilder AddAttachments(this EmbedBuilder builder, IEnumerable<IAttachment> attachments)
        //     {
        //         var sb = new StringBuilder();
        //
        //         foreach (var attachment in attachments)
        //         {
        //             sb.AppendLine($"• {Format.Bold(attachment.Filename)}");
        //             sb.Append($"{attachment.Size}px {attachment.Width}x{attachment.Height} (Spoiler: {attachment.IsSpoiler()})");
        //         }
        //
        //         return builder.AddField("Attachments", sb.ToString());
        //     }
        //
        //     public static EmbedBuilder AddChannel(this EmbedBuilder builder, IChannel channel)
        //     {
        //         return builder.AddField("Channel", $"<#{channel.Id}> ({channel.Id})");
        //     }
        //
        //     public static EmbedBuilder AddReactions(this EmbedBuilder builder,
        //         IReadOnlyDictionary<IEmote, ReactionMetadata> reactions)
        //     {
        //         var sb = new StringBuilder();
        //
        //         // ReSharper disable once UseDeconstruction
        //         foreach (var reaction in reactions)
        //         {
        //             sb.Append($"({reaction.Key.Name}, {reaction.Value.ReactionCount}), ");
        //         }
        //
        //         return builder.AddField("Reactions", sb.ToString());
        //     }
    }
}
