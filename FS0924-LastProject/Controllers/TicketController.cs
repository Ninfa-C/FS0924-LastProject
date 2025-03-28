using System.Security.Claims;
using LastProject.Data;
using LastProject.DTOs.Ticket;
using LastProject.Models.Main;
using LastProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LastProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketServices _ticketServices;
        private readonly EventServices _eventServices;

        public TicketController(TicketServices ticket, EventServices evento)
        {
            _ticketServices = ticket;
            _eventServices = evento;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AcquistaBiglietto([FromBody] NewTicketDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Utente non autenticato.");
                }
                var evento = await _eventServices.GetSingleEvent(model.EventoId);
                if (evento == null)
                {
                    return BadRequest("Evento non trovato.");
                }

                if (evento.Available < model.Quantita)
                {
                    return BadRequest("Biglietti insufficienti per questo evento.");
                }
                bool result = true;
                for (int i = 0; i < model.Quantita; i++)
                {
                    var ticket = new Ticket()
                    {
                        EventId = model.EventoId,
                        UserId = userId,
                        PurchaseDate = DateTime.UtcNow,
                    };
                    result = await _ticketServices.NewTicket(ticket);
                    if (!result)
                    {
                        result = false;
                        break;
                    }
                }
                return result ? Ok(new { Message = "Biglietti acquistati con successo!" }) : BadRequest(new { Message = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

