using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LastProject.Models.Auth;

namespace LastProject.Models.Main
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }
        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }

    }
}
