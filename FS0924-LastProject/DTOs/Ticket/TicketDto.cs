using LastProject.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LastProject.DTOs.Event;
using LastProject.DTOs.Auth;

namespace LastProject.DTOs.Ticket
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public EventDto? Event { get; set; }
        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserDto? User { get; set; }
    }
}
