using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class Infraction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public InfractionType Type { get; set; }

        [Required]
        public ulong SubjectId { get; set; }

        [Required]
        public ulong ModeratorId { get; set; }

        [Required]
        public Guild Guild { get; set; }
            
        [Required]
        public DateTime TimeInvoked { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public string Reason { get; set; }

        public bool Completed { get; set; }
    }
    
    public class InfractionConfiguration : IEntityTypeConfiguration<Infraction>
    {
        public void Configure(EntityTypeBuilder<Infraction> builder)
        {
             
            builder.HasIndex(x => x.SubjectId);
            builder.HasIndex(x => x.ModeratorId);

            builder
                .Property(x => x.SubjectId)
                .HasConversion<long>();

            builder
                .Property(x => x.ModeratorId)
                .HasConversion<long>();

            builder
                .HasOne(x => x.Guild)
                .WithMany(x => x.Infractions);
        }
    }   

    /// <summary>
    /// Represents the possible types of an <see cref="Infraction"/>.
    /// </summary>
    public enum InfractionType
    {
        Ban,
        Mute,
        Kick,
        Warning,
        Filter
    }
}