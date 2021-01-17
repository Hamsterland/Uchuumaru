using Discord;
using Discord.Commands;
using MediatR;

namespace Uchuumaru.Notifications.Commands
{
    public class CommandExecutedNotification : INotification, IDeconstructable<(Optional<CommandInfo>, ICommandContext, IResult)>
    {
        public Optional<CommandInfo> Command { get; }
        public ICommandContext Context { get; }
        public IResult Result { get; }

        public CommandExecutedNotification(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            Command = command;
            Context = context;
            Result = result;
        }

        public (Optional<CommandInfo>, ICommandContext, IResult) Deconstruct()
        {
            return (Command, Context, Result);
        }
    }
}