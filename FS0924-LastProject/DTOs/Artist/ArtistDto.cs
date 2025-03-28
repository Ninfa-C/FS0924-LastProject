using System.ComponentModel.DataAnnotations;
using LastProject.DTOs.Event;

namespace LastProject.DTOs.Artist
{
    public class ArtistDto
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(50)]
        public required string Genre { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }

        public List<EventDto>? Events { get; set; }
    }
}
