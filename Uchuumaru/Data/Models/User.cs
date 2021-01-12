using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong UserId { get; set; }
        
        [Required]
        public Guild Guild { get; set; }
    }
    
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.UserId);

            builder
                .Property(x => x.UserId)
                .HasConversion<long>();
        }
    }
}