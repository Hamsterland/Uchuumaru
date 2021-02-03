using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.Exceptions;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that allows for only moderators to execute the
    /// command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireModerator : PreconditionAttribute
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
            var author = context.User as IGuildUser;
            var database = services.GetRequiredService<UchuumaruContext>();

            var guild = await database
                .Guilds
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuildId == context.Guild.Id);

            _ = guild ?? throw new EntityNotFoundException<Guild>();

            var moderatorId = guild.ModeratorRoleId;

            return author.RoleIds.Any(roleId => roleId == moderatorId) 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError(string.Empty);
        }
    }
}