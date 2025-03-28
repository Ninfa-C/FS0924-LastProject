using LastProject.Data;
using LastProject.DTOs.Artist;
using LastProject.DTOs.Event;
using LastProject.DTOs.Ticket;
using LastProject.Models.Main;
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

        public async Task<List<Event>?> GetAll()
        {
            try
            {
                var result = await _context.Events
                    .Include(a => a.Artist)
                    .AsNoTracking()
                    .ToListAsync();
                return result;

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Create(Event model)
        {
            try
            {
                _context.Events.Add(model);
                return await _shareService.SaveAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(Guid id, Event model)
        {
            var result = await _context.Events.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            result.Titolo = model.Titolo;
            result.Location = model.Location;
            result.Price = model.Price;
            result.MaxSeats = model.MaxSeats;
            result.ArtistId = model.ArtistId;
            result.Date = model.Date;

            return await _shareService.SaveAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _context.Events.FindAsync(id);
            if (result == null) return false;

            _context.Events.Remove(result);
            return await _shareService.SaveAsync();
        }
    }
}
