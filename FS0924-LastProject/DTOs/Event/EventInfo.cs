using LastProject.DTOs.Artist;
using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Event
{
    public class EventInfo
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public ArtistInfo? Artist { get; set; }
    }
}
