using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Uchuumaru.Services.Channels
{
    public class ChannelService
    {
        private readonly DiscordSocketClient _client;

        public ChannelService(DiscordSocketClient client)
        {
            _client = client;
        }
        
        private readonly ConcurrentDictionary<ulong, OverwritePermissions> _permissions = new();

        public async Task Lock(ulong channelId)
        {
            var channel = _client.GetChannel(channelId) as IGuildChannel;
            var guild = channel.Guild;

            foreach (var overwrite in channel.PermissionOverwrites)
            {
                _permissions.TryAdd(overwrite.TargetId, overwrite.Permissions);

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (overwrite.TargetType is PermissionTarget.Role)
                {
                    var role = guild.GetRole(overwrite.TargetId);
                    await channel.AddPermissionOverwriteAsync(role,
                        new OverwritePermissions(sendMessages: PermValue.Deny));
                }

                if (overwrite.TargetType is PermissionTarget.User)
                {
                    var user = await guild.GetUserAsync(overwrite.TargetId);
                    await channel.AddPermissionOverwriteAsync(user,
                        new OverwritePermissions(sendMessages: PermValue.Deny));
                }
            }
        }

        public async Task Unlock(ulong channelId)
        {
            var channel = _client.GetChannel(channelId) as IGuildChannel;
            var guild = channel.Guild;
            
            foreach (var overwrite in channel.PermissionOverwrites)
            {
                var success = _permissions.TryGetValue(overwrite.TargetId, out var originalOverwrite);

                if (!success)
                {
                    continue;
                }

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (overwrite.TargetType is PermissionTarget.Role)
                {
                    var role = guild.GetRole(overwrite.TargetId);
                    await channel.AddPermissionOverwriteAsync(role, originalOverwrite);
                }
                
                if (overwrite.TargetType is PermissionTarget.User)
                {
                    var user = await guild.GetUserAsync(overwrite.TargetId);
                    await channel.AddPermissionOverwriteAsync(user, originalOverwrite);
                }
            }
        }
    }
}