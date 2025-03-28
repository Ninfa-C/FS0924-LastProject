using LastProject.Data;
using LastProject.Models.Main;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LastProject.Services
{
    public class TicketServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ShareService _shareService;
        public TicketServices(ApplicationDbContext context, ShareService shareService, ILogger<TicketServices> logger)
        {
            _context = context;
            _shareService = shareService;
        }


        public async Task<bool> NewTicket(Ticket model)
        {
            try
            {
                _context.Tickets.Add(model);
                return await _shareService.SaveAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Ticket>?> GetMyTickets(string id)
        {
            try
            {
                return await _context.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Artist)
                    .Include(t => t.User)
                    .Where(t => t.UserId == id)
                    .ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(Guid id, string userId)
        {
            Log.Warning("Tentativo di eliminazione biglietto ID: {TicketId} da parte dell'utente {UserId}", id, userId);
            var result = await _context.Tickets.FindAsync(id);
            if (result == null) return false;
            if (result.UserId != userId)
            {
                Log.Warning("Tentativo non autorizzato di eliminazione biglietto ID: {TicketId} da parte di {UserId}", id, userId);
                return false;
            }

            _context.Tickets.Remove(result);
            await _shareService.SaveAsync();
            Log.Information("Biglietto ID: {TicketId} eliminato con successo", id);
            return true;
        }

        public async Task<List<Ticket>> GetAll()
        {
            try
            {
                return await _context.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Artist)
                    .Include(t => t.User)
                    .ToListAsync();

            }
            catch
            {
                return null;
            }
        }

    }
}
