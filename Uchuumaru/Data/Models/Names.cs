using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Uchuumaru.Data.Models
{
    public class Username
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User User { get; set; }
        
        public string Value { get; set; }
    }

    public class UsernameConfiguration : IEntityTypeConfiguration<Username>
    {
        public void Configure(EntityTypeBuilder<Username> builder)
        {
            builder.HasIndex(x => x.Value);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Usernames);
        }
    }
    
    public class Nickname
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public User User { get; set; }
        
        public string Value { get; set; }
    }
    
    public class NicknameConfiguration : IEntityTypeConfiguration<Nickname>
    {
        public void Configure(EntityTypeBuilder<Nickname> builder)
        {
            builder.HasIndex(x => x.Value);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Nicknames);
        }
    }
}