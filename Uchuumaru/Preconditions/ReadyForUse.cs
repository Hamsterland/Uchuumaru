using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    public class ReadyForUse : PreconditionAttribute
    {
        private readonly bool _usable;

        public ReadyForUse(bool usable)
        {
            _usable = usable;
        }
        
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return _usable
                ? Task.FromResult(PreconditionResult.FromSuccess())
                : Task.FromResult(PreconditionResult.FromError("This command is not usable yet."));
        }
    }
}