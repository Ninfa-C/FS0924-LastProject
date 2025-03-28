using LastProject.Data;
using LastProject.Models.Main;

namespace LastProject.Services
{
    public class TicketServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ShareService _shareService;

        public TicketServices(ApplicationDbContext context, ShareService shareService)
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
    }
}
