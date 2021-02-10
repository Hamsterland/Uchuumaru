using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Uchuumaru.Data;
using Uchuumaru.Notifications;

namespace Uchuumaru.Services.Messages
{
    public class MessageUpdatedListener : INotificationHandler<MessageUpdatedNotification>
    {
        private readonly UchuumaruContext _uchuumaruContext;

        public MessageUpdatedListener(UchuumaruContext uchuumaruContext)
        {
            _uchuumaruContext = uchuumaruContext;
        }

        public async Task Handle(MessageUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var messageBefore = notification.Before;
            var messageAfter = notification.After;
            var sourceChannel = notification.Channel;
            
            var sourceGuild = (sourceChannel as IGuildChannel).Guild;
            var sourceGuildId = sourceGuild.Id;

            var guild = await _uchuumaruContext
                .Guilds
                .FirstOrDefaultAsync(x => x.GuildId == sourceGuildId, cancellationToken);

            var messageLogChannelId = guild.MessageChannelId; 

            if (messageLogChannelId == 0)
            {
                return;
            }

            if (!messageBefore.HasValue)
            {
                return;
            }

            if (messageBefore.Value.Author.IsBot)
            {
                return;
            }
            
            var flags = 0;
            var author = messageAfter.Author;
                    
            var embed = new EmbedBuilder()
                .WithTitle("Message Updated")
                .WithColor(Constants.DefaultColour)
                .AddField("Channel", $"{sourceChannel.Name} ({sourceChannel.Id}) [Link]({messageAfter.GetJumpUrl()})")
                .AddMessageAuthor(author);
            
            var beforeContent = messageBefore.Value.Content;
            var afterContent = messageAfter.Content;
                    
            if (beforeContent != afterContent)
            {
                embed.AddContent(beforeContent, afterContent);
                flags++; 
            }

            var beforePinned = messageBefore.Value.IsPinned;
            var afterPinned = messageAfter.IsPinned;

            if (beforePinned != afterPinned)
            {
                embed.AddPinned(beforePinned, afterPinned);
                flags++;
            }

            if (flags > 0)
            {
                var messageLogChannel = await sourceGuild.GetChannelAsync(messageLogChannelId) as IMessageChannel;
                await messageLogChannel.SendMessageAsync(embed: embed.Build());
            }
        }
    }

    public static class MessageUpdatedExtensions
    {
        public static EmbedBuilder AddContent(this EmbedBuilder builder, string before, string after)
        {
            var c1 = before;
            var c2 = after;

            if (c1.Length > 1024)
            {
                c1 = new string($"{before.Take(1017).ToArray()}...");
            }

            if (c2.Length > 1024)
            {
                c2 = new string($"{after.Take(1017).ToArray()}...");
            }

            return builder
                .AddField("Before", c1)
                .AddField("After", c2);
        }

        public static EmbedBuilder AddPinned(this EmbedBuilder builder, bool before, bool after)
        {
            return builder.AddField("Pinned", $"{before} to {after}");
        }
    }
}