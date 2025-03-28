using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Ticket
{
    public class NewTicketDto
    {
        [Required]
        public Guid EventoId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Quantita { get; set; } = 1;
    }
}
