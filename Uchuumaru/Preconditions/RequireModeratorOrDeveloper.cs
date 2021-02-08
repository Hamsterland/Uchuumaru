using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that allows for only developers or moderators
    /// to execute a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireModeratorOrDeveloper : PreconditionAttribute
    {
        /// <summary>
        /// Checks if the author has the correct permissions to execute the command. The command is
        /// otherwise not pushed to execution.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="command">The command.</param>
        /// <param name="services">The service collection.</param>
        /// <returns>
        /// /// A <see cref="PreconditionResult"/> that determines whether the command should be pushed
        /// to execution or not. 
        /// </returns>
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var requireDeveloper = new RequireDeveloper();
            var developer = await requireDeveloper.CheckPermissionsAsync(context, command, services);
            
            if (developer.IsSuccess)
            {
                return PreconditionResult.FromSuccess();
            }

            var requireModerator = new RequireModerator();
            var moderator = await requireModerator.CheckPermissionsAsync(context, command, services);
            
            return moderator.IsSuccess 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError(string.Empty);
        }
    }
}