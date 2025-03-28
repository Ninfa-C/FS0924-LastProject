using System.ComponentModel.DataAnnotations;
using LastProject.DTOs.Artist;
using LastProject.DTOs.Ticket;

namespace LastProject.DTOs.Event
{
    public class EventDto
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

        public ArtistDto? Artist { get; set; }
        public int Available { get; set; }

    }
}
