using LastProject.DTOs.Artist;
using LastProject.DTOs.Event;
using LastProject.Models.Main;
using LastProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LastProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {

        public readonly EventServices _eventServices;

        public EventController(EventServices eventServices)
        {
            _eventServices = eventServices;
        }

        [Authorize(Roles = "Amministratore")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddEvent model)
        {
            try
            {
                var newEvent = new Event()
                {
                    Titolo = model.Titolo,
                    Date = DateTime.Now,
                    Location = model.Location,
                    Price = model.Price,
                    MaxSeats = model.MaxSeats,
                    ArtistId = model.ArtistId
                };
                var result = await _eventServices.Create(newEvent);
                return result ? Ok(new { Message = "Evento aggiunto!" }) : BadRequest(new { Message = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var result = await _eventServices.GetSingleEvent(id);

            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }

            return Ok(new
            {
                data = result
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _eventServices.GetAll();

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

            var text = count == 1 ? $"{count} evento trovato" : $"{count} eventi trovati";

            var evento = result.Select(s => new EventDto
            {
                Id = s.Id,
                Name = s.Titolo,
                Date = s.Date,
                Location = s.Location,
                Price = s.Price,
                Available = s.MaxSeats - (s.Tickets != null ? s.Tickets.Count : 0),
                Artist = s.Artist != null ? new ArtistDto
                {
                    Name = s.Artist.Name,
                    Genre = s.Artist.Genre,
                    Description = s.Artist.Description!,
                    Id = s.Artist.Id
                } : null,
            }).ToList();

            return Ok(new
            {
                message = text,
                data = evento
            });
        }

        [Authorize(Roles = "Amministratore")]
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] EditEvent model)
        {
            var evento = new Event
            {
                Id = id,
                Titolo = model.Titolo,
                Location = model.Location,
                Price = model.Price,
                MaxSeats = model.MaxSeats,
                ArtistId = model.ArtistId,
                Date = model.Date
            };
            var result = await _eventServices.Update(id, evento);

            return result ? Ok(new { message = "Evento aggiornato!" }) : BadRequest(new { message = "Something went wrong" });
        }



        [Authorize(Roles = "Amministratore")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _eventServices.Delete(id);
            return result ? Ok(new { message = "Evento eliminato!" }) : BadRequest(new { message = "Something went wrong" });
        }

    }
}
