using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that allows for the command to only be
    /// executed if the context Guild is MyAnimeList.
    /// </summary>
    public class MyAnimeListOnly : PreconditionAttribute
    {
        private const ulong _myAnimeListId = 301123999000166400;
        
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
            return context.Guild.Id == _myAnimeListId
                ? Task.FromResult(PreconditionResult.FromSuccess())
                : Task.FromResult(PreconditionResult.FromError(string.Empty));
        }
    }
}