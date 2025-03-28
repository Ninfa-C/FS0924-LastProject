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
    [Authorize]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistServices _artistServices;
        public readonly EventServices _eventServices;

        public ArtistController(ArtistServices services, EventServices eventServices)
        {
            _artistServices = services;
            _eventServices = eventServices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _artistServices.GetAll();

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

            var text = count == 1 ? $"{count} artista trovato" : $"{count} artisti trovati";

            var artist = result.Select(s => new ArtistDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description!,
                Genre = s.Genre,
                Events = s.Events != null ? s.Events.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Titolo,
                    Date = e.Date,
                    Location = e.Location,
                    Price = e.Price,
                    Available = e.MaxSeats - e.Tickets!.Count,
                }).ToList() : null,
            }).ToList();

            return Ok(new
            {
                message = text,
                data = artist
            });
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var result = await _artistServices.GetOne(id);

            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }

            var artist = new ArtistDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description!,
                Genre = result.Genre,
                Events = result.Events != null ? result.Events.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Titolo,
                    Date = e.Date,
                    Location = e.Location,
                    Price = e.Price,
                    Available = e.MaxSeats - e.Tickets!.Count,
                }).ToList() : null,
            };

            return Ok(new
            {
                data = artist
            });
        }

        [Authorize(Roles = "Amministratore")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddArtist model)
        {
            try
            {
                var newArtist = new Artist()
                {
                    Name = model.Name,
                    Genre = model.Genre,
                    Description = model.Description,
                    Events = null
                };

                var result = await _artistServices.Create(newArtist);
                return result ? Ok(new { Message = "Artista aggiunto!" }) : BadRequest(new { Message = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Roles = "Amministratore")]
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] EditArtist model)
        {
            var artist = new Artist
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Genre = model.Genre,
            };
            var result = await _artistServices.Update(id, artist);

            return result ? Ok(new { message = "Artista aggiornato!" }) : BadRequest(new { message = "Something went wrong" });
        }



        [Authorize(Roles = "Amministratore")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _artistServices.Delete(id);
            return result ? Ok(new { message = "Artista eliminato!" }) : BadRequest(new { message = "Something went wrong" });
        }

    }
}
