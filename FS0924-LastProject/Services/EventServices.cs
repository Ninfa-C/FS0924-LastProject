using LastProject.Data;
using LastProject.DTOs.Artist;
using LastProject.DTOs.Event;
using LastProject.DTOs.Ticket;
using Microsoft.EntityFrameworkCore;

namespace LastProject.Services
{
    public class EventServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ShareService _shareService;
        public EventServices(ApplicationDbContext context, ShareService shareService)
        {
            _context = context;
            _shareService = shareService;
        }

        public async Task<EventDto?> GetSingleEvent(Guid id)
        {
            try
            {
                var result = await _context.Events
                    .Include(e => e.Tickets)
                    .Include(e => e.Artist)
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (result == null)
                {
                    return null;
                }

                var evento = new EventDto()
                {
                    Id = result.Id,
                    Name = result.Titolo,
                    Date = result.Date,
                    Location = result.Location,
                    Price = result.Price,
                    Artist = result.Artist != null ? new ArtistDto()
                    {
                        Name = result.Artist.Name,
                        Genre = result.Artist.Genre,
                        Description = result.Artist.Description!,
                        Id = result.Artist.Id
                    } : null,
                    Available = result.MaxSeats - result.Tickets!.Count,
                };
                return evento;
            }

            catch { return null; }
        }
    }
}
