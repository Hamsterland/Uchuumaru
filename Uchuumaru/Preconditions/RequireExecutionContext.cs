using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Uchuumaru.Preconditions
{
    /// <summary>
    /// A <see cref="PreconditionAttribute"/> that determines if a command should
    /// be executed depending on the supplied <see cref="ExecutionContext"/>.
    /// </summary>
    [Obsolete("DO NOT USE THIS ITS NOT READY PLEASE.")]
    public class RequireExecutionContext : PreconditionAttribute
    {
        /// <summary>
        /// The context in which the command should be executed in.
        /// </summary>
        private ExecutionContext _executionContext;
        
        /// <summary>
        /// Constructs a new <see cref="RequireExecutionContext"/>.
        /// </summary>
        /// <param name="executionContext">The context in which the command should be executed in.</param>
        public RequireExecutionContext(ExecutionContext executionContext)
        {
            _executionContext = executionContext;
        }
        
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
            var host = services.GetRequiredService<IHostEnvironment>();
            const string error = "This command requires a {0} environment, but the current environment is {1}.";

            return _executionContext switch
            {
                ExecutionContext.DEVELOPMENT when host.IsDevelopment() 
                    => Task.FromResult(PreconditionResult.FromSuccess()),
                
                ExecutionContext.DEVELOPMENT when host.IsProduction() 
                    => Task.FromResult(PreconditionResult.FromError(string.Format(error, "development", "production"))),
                
                ExecutionContext.PRODUCTION when host.IsProduction() 
                    => Task.FromResult(PreconditionResult.FromSuccess()),
                
                ExecutionContext.PRODUCTION when host.IsDevelopment() 
                    => Task.FromResult(PreconditionResult.FromError(string.Format(error, "production", "development"))),
                
                _ => throw new InvalidEnumArgumentException(nameof(_executionContext))
            };
        }
    }

    /// <summary>
    /// Represents various contexts to limit when certain code
    /// can be executed.
    /// </summary>
    public enum ExecutionContext
    {
        /// <summary>
        /// In a development context.
        /// </summary>
        DEVELOPMENT,
        
        /// <summary>
        /// In a production context.
        /// </summary>
        PRODUCTION
    }
}