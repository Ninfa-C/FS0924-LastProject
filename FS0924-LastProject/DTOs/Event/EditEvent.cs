using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Event
{
    public class EditEvent
    {
        [Required]
        [StringLength(150)]
        public required string Titolo { get; set; }
        [Required]
        [StringLength(200)]
        public required string Location { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }
        [Required]
        public int MaxSeats { get; set; }
        [Required]
        public Guid ArtistId { get; set; }
    }
}
