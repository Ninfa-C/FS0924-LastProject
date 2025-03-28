using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LastProject.Models.Main
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(150)]
        public required string Titolo { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(200)]
        public required string Location { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }
        [Required]
        public int MaxSeats { get; set; }
        [Required]
        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        public Artist? Artist { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
