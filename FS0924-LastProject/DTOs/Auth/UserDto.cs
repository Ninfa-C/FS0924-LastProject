using LastProject.DTOs.Ticket;
using LastProject.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Auth
{
    public class UserDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Surname { get; set; }

        public ICollection<TicketDto>? Tickets { get; set; }
    }
}
