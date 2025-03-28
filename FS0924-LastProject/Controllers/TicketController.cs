using System.Security.Claims;
using LastProject.Data;
using LastProject.DTOs.Artist;
using LastProject.DTOs.Auth;
using LastProject.DTOs.Event;
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

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult> GetMyTickets()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Utente non autenticato.");
                }

                var tickets = await _ticketServices.GetMyTickets(userId);
                if (tickets == null || !tickets.Any())
                {
                    return NotFound("Nessun biglietto trovato.");
                }

                var result = tickets.Select(t => new MyTicket
                {
                    TicketId = t.Id,
                    ArtistName = t.Event.Artist.Name,
                    UserName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                    EventName = t.Event.Titolo,
                    Date = t.Event.Date,
                    Location = t.Event.Location,
                    Price = t.Event.Price,
                }).ToList();

                return Ok(result);
            }
            catch
            {
                return BadRequest("Something went wrong.");
            }
        }


        [Authorize(Roles = "Amministratore")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ticketServices.GetAll();

            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Qualcosa è andato storto!"
                });
            }

            if (!result.Any())
            {
                return NoContent();
            }

            var count = result.Count();

            var text = count == 1 ? $"{count} biglietto trovato" : $"{count} biglietti trovati";

            var model = result.Select(s => new TicketDto
            {
                Id = s.Id,
                User = new UserInfo
                {
                    UserName = s.User!.Name,
                    UserSurame = s.User.Surname,
                    Email = s.User.Email!,
                    UserId = s.UserId
                },
                Event = new EventInfo()
                {
                    Id = s.Event!.Id,
                    Name = s.Event.Titolo,
                    Date = s.Event.Date,
                    Location = s.Event.Location,
                    Price = s.Event.Price,
                    Artist = new ArtistInfo()
                    {
                        Id = s.Event.Artist.Id,
                        Name = s.Event.Artist!.Name,
                        Description = s.Event.Artist.Description!,
                        Genre = s.Event.Artist.Genre
                    }
                }
            });

            return Ok(new
            {
                message = text,
                data = model,
            });
        }


        [Authorize]
        [HttpDelete("profile/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Utente non autenticato.");
                }
                var result = await _ticketServices.Delete(id, userId);
                return result ? Ok(new { message = "Biglietto eliminato!" }) : BadRequest(new { message = "Something went wrong" });
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }


        //NON è possibile procedere con la modifica del biglietto, il cliente se vuole può cancellare lordine,
        //che verrà rimborsato nel giro di 3/5 giorni lavorativi e procedere con uno nuovo, grazie della pazienza :) 
    }
}

