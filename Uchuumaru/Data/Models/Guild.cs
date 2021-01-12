using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Uchuumaru.Data.Models
{
    public class Guild
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong GuildId { get; set; }

        [Required]
        public List<Channel> Channels { get; set; }
    }
}