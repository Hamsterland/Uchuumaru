using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.Infractions
{
    /// <summary>
    /// Describes a service that manages infractions, particularly database-side.
    /// </summary>
    public interface IInfractionService
    {
        /// <summary>
        /// Creates a new infraction.
        /// </summary>
        /// <param name="type">The type of infraction.</param>
        /// <param name="guildId">The guild the infraction belongs to.</param>
        /// <param name="subjectId">The user subject to the infraction.</param>
        /// <param name="moderatorId">The moderator performing the infraction.</param>
        /// <param name="duration">The duration of the infraction.</param>
        /// <param name="reason">The reason for the infraction.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task<int> CreateInfraction(
            InfractionType type, 
            ulong guildId, 
            ulong subjectId, 
            ulong moderatorId, 
            TimeSpan duration, 
            string reason = null);

        /// <summary>
        /// Sets or removes the <see cref="Guild.FilterChannelId"/> property in the database.
        /// Throws an <see cref="InvalidInfractionChannelException"/> if the specified channel is
        /// not an <see cref="Discord.ITextChannel"/>.
        /// </summary>
        /// <param name="options">Whether to set or remove.</param>
        /// <param name="guildId">The target Guild Id.</param>
        /// <param name="channelId">The target channel Id.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task ModifyInfractionChannel(ChannelModificationOptions options, ulong guildId, ulong channelId = 0);
        
        /// <summary>
        /// Retrieves the <see cref="Guild.InfractionChannelId"/> property value. 
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <returns>
        /// The Id of the guild infraction channel. 
        /// </returns>
        Task<ulong> GetInfractionChannelId(ulong guildId);

        /// <summary>
        /// Gets the <see cref="IUserMessage"/> representation of an infraction from the
        /// infraction channel.
        /// </summary>
        /// <param name="guildId">The guild Id.</param>
        /// <param name="messageId">The message Id.</param>
        /// <returns>
        /// The infraction message.
        /// </returns>
        Task<IUserMessage> GetInfractionMessage(ulong guildId, ulong messageId);

        /// <summary>
        /// Claims an infraction by modifying the infraction channel message and
        /// updating the database. Throws an <see cref="InfractionMessageNotFoundException"/> if the
        /// infraction message cannot be found.
        /// </summary>
        /// <param name="guildId">The guild ID.</param>
        /// <param name="messageId">The infraction message ID.</param>
        /// <param name="moderatorId">The moderator ID.</param>
        /// /// <param name="reason">The infraction reason.</param>
        /// <returns>
        /// A <see cref="Task"/> that returns upon completion.
        /// </returns>
        Task ClaimInfraction(ulong guildId, ulong messageId, ulong moderatorId, string reason = null);

        /// <summary>
        /// Gets all the infractions for a user in a guild.
        /// </summary>
        /// <param name="guildId">The guild ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>
        /// A list of all infractions.
        /// </returns>
        Task<IEnumerable<InfractionSummary>> GetAllInfractions(ulong guildId, ulong userId);
    }
}