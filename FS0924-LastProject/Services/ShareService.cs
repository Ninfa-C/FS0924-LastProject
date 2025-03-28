using LastProject.Data;
using Microsoft.EntityFrameworkCore;

namespace LastProject.Services
{
    public class ShareService
    {
        private readonly ApplicationDbContext _context;

        public ShareService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<bool> SaveAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync() > 0;
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
