using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Infractions.Warns
{
    public class WarnService : IWarnService
    {
        private readonly IInfractionService _infraction;

        private readonly DiscordSocketClient _client;
        
        public WarnService(IInfractionService infraction, DiscordSocketClient client)
        {
            _infraction = infraction;
            _client = client;
        }

        public async Task CreateWarn(
            ulong guildId,
            ulong subjectId,
            ulong moderatorId,
            string reason = null)
        {
            var id = await _infraction.CreateInfraction(InfractionType.Warning, guildId, subjectId, moderatorId, TimeSpan.Zero, reason);
            
            var infractionChannelId = await _infraction.GetInfractionChannelId(guildId);
            
            if (infractionChannelId == 0)
            {
                return;  
            }
            
            if (_client.GetChannel(infractionChannelId) is not ITextChannel infractionChannel)
            {
                return; 
            }

            var user = _client.GetUser(subjectId);
            var moderator = _client.GetUser(moderatorId);
            
            var builder = new InfractionEmbedBuilder("User Warned", id, user, moderator, reason);
            var message = await infractionChannel.SendMessageAsync(embed: builder.Build());

            if (reason is null)
            {
                builder.Reason = $"+infraction claim {message.Id} <reason>";
            }
            
            await message.ModifyAsync(props => props.Embed = builder.Build());
        }
    }
}