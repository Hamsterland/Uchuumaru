using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    public class RequireGuild : PreconditionAttribute
    {
        public ulong[] AcceptedGuilds { get; }

        public RequireGuild(params ulong[] acceptedGuilds)
        {
            AcceptedGuilds = acceptedGuilds;
        }
        
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return Task.FromResult(AcceptedGuilds.Contains(context.Guild.Id)
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError("You cannot use this command in this guild."));
        }
    }
}