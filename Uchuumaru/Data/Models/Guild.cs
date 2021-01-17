using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class Guild
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong GuildId { get; set; }

        [Required]
        public bool EnabledFilter { get; set; }

        public List<Filter> Filters { get; set; } = new();

        public ulong FilterChannelId { get; set; }

        public List<Infraction> Infractions { get; set; } = new();

        public ulong InfractionChannelId { get; set; }
    }
    
    public class GuildConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder
                .HasIndex(x => x.GuildId)
                .IsUnique();

            builder
                .Property(x => x.GuildId)
                .HasConversion<long>();

            builder
                .Property(x => x.FilterChannelId)
                .HasConversion<long>();
        }
    }
}