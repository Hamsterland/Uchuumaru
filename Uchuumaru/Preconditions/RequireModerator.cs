using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    public class RequireModerator : PreconditionAttribute
    {
        private const ulong _moderatorId = 301125242749714442;
        
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var author = context.User as IGuildUser;

            return Task.FromResult(author.RoleIds.Any(roleId => roleId == _moderatorId) 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError(string.Empty));
        }
    }
}