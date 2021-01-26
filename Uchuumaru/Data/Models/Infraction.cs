using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
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
        public DateTime TimeInvoked { get; set; } = DateTime.UtcNow;
        
        public TimeSpan Duration { get; set; }
        
        public string Reason { get; set; }

        public bool Completed { get; set; }
        
        [NotMapped]
        public TimeSpan RemainingTime => TimeInvoked.Add(Duration).Subtract(DateTime.UtcNow);

        [NotMapped]
        public TimeSpan ElapsedTime => Duration.Subtract(RemainingTime);
        
        public InfractionSummary ToInfractionSummary()
        {
            return new()
            {
                Id = Id,
                Type = Type,
                SubjectId = SubjectId,
                ModeratorId = ModeratorId,
                Guild = Guild,
                TimeInvoked = TimeInvoked,
                Duration = Duration,
                Reason = Reason,
                Completed = Completed
            }; 
        }
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

    public class InfractionSummary
    {
        public int Id { get; init; }
        public InfractionType Type { get; init; }
        public ulong SubjectId { get; init; }
        public ulong ModeratorId { get; init; }
        public Guild Guild { get; init; }
        public DateTime TimeInvoked { get; init; }
        public TimeSpan Duration { get; init; }
        public string Reason { get; init; }
        public bool Completed { get; init; }

        public static readonly Expression<Func<Infraction, InfractionSummary>> FromEntityProjection = infraction => new InfractionSummary
            {
                Id = infraction.Id,
                Type = infraction.Type,
                SubjectId = infraction.SubjectId,
                ModeratorId = infraction.ModeratorId,
                Guild = infraction.Guild,
                TimeInvoked = infraction.TimeInvoked,
                Duration = infraction.Duration,
                Reason = infraction.Reason,
                Completed = infraction.Completed
            };
    }
}