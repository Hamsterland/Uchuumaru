using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that allows a command to execute if
    /// the author has any of the specified <see cref="Roles"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoles : PreconditionAttribute
    {
        /// <summary>
        /// The allowed roles where the author have at least on of them.
        /// </summary>
        public ulong[] Roles { get; }

        /// <summary>
        /// Creates a new <see cref="RequireRoles"/>.
        /// </summary>
        /// <param name="roles">The allowed roles.</param>
        public RequireRoles(params ulong[] roles)
        {
            Roles = roles;
        }
        
        /// <summary>
        /// Checks the permissions.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="command">The command.</param>
        /// <param name="services">The service collection.</param>
        /// <returns>
        /// A <see cref="PreconditionResult"/> that determines whether the command
        /// should push to execution or not.
        /// </returns>
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var author = context.User as IGuildUser;

            return Task.FromResult(Roles.Any(roleId => author.RoleIds.Contains(roleId)) 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError(string.Empty));
        }
    }
}