using System;

namespace Uchuumaru.Services.Infractions
{
    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when a specified channel to be set as
    /// the infraction channel is invalid.
    /// </summary>
    public class InvalidInfractionChannelException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InvalidInfractionChannelException"/>.
        /// </summary>
        /// <param name="name">The channel name.</param>
        /// <param name="channelId">The channel Id.</param>
        public InvalidInfractionChannelException(string name, ulong channelId) : base($"The channel \"{name}\" ({channelId}) is not a text channel.")
        { }
    }

    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when a specified infraction
    /// message is not found in the infraction channel.
    /// </summary>
    public class InfractionMessageNotFoundException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InfractionMessageNotFoundException"/>.
        /// </summary>
        /// <param name="messageId">The infraction message ID.</param>
        public InfractionMessageNotFoundException(ulong messageId) : base($"No infraction message with ID \"{messageId}\" could be found.")
        { }
    }

    /// <summary>
    /// Represents an <see cref="Exception"/> thrown when an Infraction with the specified
    /// ID could not be found.
    /// </summary>
    public class InfractionNotFoundException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InfractionNotFoundException"/>.
        /// </summary>
        /// <param name="id">The ID of the Infraction.</param>
        public InfractionNotFoundException(int id) : base($"No infraction with ID \"{id}\" could be found.")
        { }
    }
}