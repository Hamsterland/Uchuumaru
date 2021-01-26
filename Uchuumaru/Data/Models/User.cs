using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public List<Username> Usernames { get; set; } = new();
        public List<Nickname> Nicknames { get; set; } = new();

        public UserSummary ToUserSummary()
        {
            return new UserSummary
            {
                UserId = UserId,
                GuildId = Guild.GuildId,
                Usernames = Usernames.Select(username => username.Value.ToString()).ToList(),
                Nicknames = Nicknames.Select(nickname => nickname.Value.ToString()).ToList(),
            }; 
        }
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

    public class UserSummary
    {
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Nicknames { get; set; }
    }
}