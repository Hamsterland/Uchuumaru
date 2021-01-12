using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Uchuumaru.Data.Models
{
    public class Filter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guild Guild { get; set; }

        [Required]
        public string Expression { get; set; }
    }

    public class FilterConfiguration : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> builder)
        {
            builder
                .HasOne(x => x.Guild)
                .WithMany(x => x.Filters);
        }
    }
}