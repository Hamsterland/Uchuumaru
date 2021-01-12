using System.ComponentModel.DataAnnotations;

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
}