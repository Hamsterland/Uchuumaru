using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class Filter
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public Guild Guild { get; set; }

        [Required] 
        public string Expression { get; set; }
    }

    public class FilterConfiguration : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> builder)
        {
            builder
                .HasOne(x => x.Guild)
                .WithMany(x => x.Filters);
        }
    }

    /// <summary>
    /// A data transfer object for the <see cref="Filter"/> model that describes Filter
    /// information for a guild.
    /// </summary>
    public class FilterStatus
    {
        /// <summary>
        /// The guild Id.
        /// </summary>
        public ulong GuildId { get; init; }

        /// <summary>
        /// Whether the Filter is enabled or not.
        /// </summary>
        public bool Enabled { get; init; }

        /// <summary>
        /// The filtered expressions.
        /// </summary>
        public List<string> Expressions { get; set; }
    }
}