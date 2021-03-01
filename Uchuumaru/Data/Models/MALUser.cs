using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class MALUser
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public ulong UserId { get; set; }
        public int MAL { get; set; }
        public string VerificationCode { get; set; }

        [NotMapped]
        public bool IsVerified => MAL != 0;
    }
    
    public class MALUserConfiguration : IEntityTypeConfiguration<MALUser>
    {
        public void Configure(EntityTypeBuilder<MALUser> builder)
        {
            builder
                .Property(x => x.UserId)
                .HasConversion<long>();
            
            builder
                .HasIndex(x => x.UserId)
                .IsUnique();
            
            builder
                .HasIndex(x => x.MAL)
                .IsUnique();
            
            builder
                .HasIndex(x => x.VerificationCode)
                .IsUnique();
        }
    }
}