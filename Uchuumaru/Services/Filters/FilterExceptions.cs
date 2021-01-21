using System;

namespace Uchuumaru.Services.Filters
{
    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when a specified expression does not exist.
    /// </summary>
    public class ExpressionExistsException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="ExpressionExistsException"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public ExpressionExistsException(string expression) : base($"Expression \"{expression}\"already exists.")
        {
        }
    }
    
    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when a specified expression already exists.
    /// </summary>
    public class ExpressionDoesNotExistException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="ExpressionExistsException"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public ExpressionDoesNotExistException(string expression) : base($"Expression \"{expression}\" does not exist.")
        {
        }
    }

    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when a specified channel to be set as
    /// the filter channel is invalid.
    /// </summary>
    public class InvalidFilterChannelException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InvalidFilterChannelException"/>.
        /// </summary>
        /// <param name="name">The channel name.</param>
        /// <param name="channelId">The channel Id.</param>
        public InvalidFilterChannelException(string name, ulong channelId) : base($"The channel \"{name}\" ({channelId}) is not a text channel.")
        {
        }
    }
}