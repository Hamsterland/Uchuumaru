using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that allows for only Developers to use the module or command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireDeveloper : PreconditionAttribute
    {
        /// <summary>
        /// Determines if the executor of the command is a Developer. If not, the command is not executed.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="command">The command to be executed.</param>
        /// <param name="services">The service collection.</param>
        /// <returns>
        /// A <see cref="PreconditionResult"/> that determines whether the command should be pushed to
        /// execution or not. 
        /// </returns>
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var configuration = services.GetService<IConfiguration>();

            var developers = configuration
                .GetSection("Developers")
                .AsEnumerable()
                .Select(x => x.Value)
                .ToList();

            developers.RemoveAt(0);
            
            var ids = developers.Select(ulong.Parse);
            var author = context.Message.Author;
            
            return Task.FromResult(ids.Any(x => x == author.Id) 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError($"{author} {author.Id} is not a developer."));
        }
    }
}