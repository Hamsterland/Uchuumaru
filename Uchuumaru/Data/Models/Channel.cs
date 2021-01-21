using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class Channel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong ChannelId { get; set; }

        [Required]
        public ulong GuildId { get; set; }
    }
    
    public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder
                .HasIndex(x => x.ChannelId)
                .IsUnique();

            builder.HasIndex(x => x.GuildId);

            builder
                .Property(x => x.ChannelId)
                .HasConversion<long>();

            builder
                .Property(x => x.GuildId)
                .HasConversion<long>();
        }
    }
    
    public enum ChannelModificationOptions
    {
        Set,
        Remove
    }
}