using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace LastProject.Models.Main
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        [Required]
        [StringLength(100)]
        public required string Genre { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public List<Event>? Events { get; set; }
    }
}
