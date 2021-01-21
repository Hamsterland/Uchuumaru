using System.Linq;
using Discord;

namespace Uchuumaru.Services.Infractions
{
    /// <summary>
    /// Provides a high-level abstraction over an <see cref="EmbedBuilder"/> specifically for
    /// infraction notices. 
    /// </summary>
    public class InfractionEmbedBuilder : EmbedBuilder
    {
        /// <summary>
        /// Constructs a new <see cref="InfractionEmbedBuilder"/>.
        /// </summary>
        /// <param name="title">The notice title.</param>
        /// <param name="id">The infraction ID.</param>
        /// <param name="subject">The infraction subject.</param>
        /// <param name="moderator">The infraction moderator.</param>
        /// <param name="reason">The infraction reason.</param>
        public InfractionEmbedBuilder(
            string title,
            int id, 
            IUser subject,
            IUser moderator = null, 
            string reason = null)
        {
            WithTitle(title);
            WithColor(Constants.DefaultColour);
            AddField("Case", id);
            AddField("Subject", $"{subject} ({subject.Id})");
           
            AddField("Moderator", moderator is not null 
                ? $"{moderator} ({moderator.Id})" 
                : "Placeholder");

            AddField("Reason", reason ?? "Placeholder");
        }

        /// <summary>
        /// Constructs a new <see cref="InfractionEmbedBuilder"/> from an
        /// existing embed.
        /// </summary>
        /// <param name="embed">The embed.</param>
        public InfractionEmbedBuilder(IEmbed embed)
        {
            WithTitle(embed.Title);
            WithColor(Constants.DefaultColour);

            foreach (var field in embed.Fields.Where(field => field.Name != null && field.Value != null))
            {
                AddField(field.Name, field.Value);
            }
        }
        
        /// <summary>
        /// The case ID.
        /// </summary>
        public int Case
        {
            get => int.Parse(Fields.FirstOrDefault(x => x.Name == "Case").Value.ToString()!);
            set => Fields.FirstOrDefault(x => x.Name == "Case").Value = value;
        }
        
        /// <summary>
        /// The subject.
        /// </summary>
        public string Subject
        {
            get => Fields.FirstOrDefault(x => x.Name == "Subject").ToString();
            set => Fields.FirstOrDefault(x => x.Name == "Subject").Value = value;
        }
        
        /// <summary>
        /// The moderator.
        /// </summary>
        public string Moderator
        {
            get => Fields.FirstOrDefault(x => x.Name == "Moderator")?.ToString();
            set => Fields.FirstOrDefault(x => x.Name == "Moderator").Value = value;
        }
        
        /// <summary>
        /// The reason.
        /// </summary>
        public string Reason
        {
            get => Fields.FirstOrDefault(x => x.Name == "Reason")?.ToString();
            set => Fields.FirstOrDefault(x => x.Name == "Reason").Value = value;
        }
    }
}