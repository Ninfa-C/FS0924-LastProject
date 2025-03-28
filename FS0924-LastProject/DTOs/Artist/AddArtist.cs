using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Artist
{
    public class AddArtist
    {
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(50)]
        public required string Genre { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }

    }
}
