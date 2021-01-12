using System.ComponentModel.DataAnnotations;

namespace Uchuumaru.Data.Models
{
    public class Channel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong ChannelId { get; set; }
        
        [Required]
        public Guild Guild { get; set; }
    }
}