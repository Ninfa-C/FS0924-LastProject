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
        public EventInfo Event { get; set; }
        public UserInfo User { get; set; }
    }
}
