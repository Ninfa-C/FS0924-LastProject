using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Artist
{
    public class ArtistInfo
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(50)]
        public required string Genre { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }
    }
}
