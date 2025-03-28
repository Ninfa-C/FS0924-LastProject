using LastProject.Data;
using LastProject.DTOs.Artist;
using LastProject.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace LastProject.Services
{

    public class ArtistServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ShareService _shareService;
        public ArtistServices(ApplicationDbContext context, ShareService shareService)
        {
            _context = context;
            _shareService = shareService;
        }

        public async Task<bool> New(Artist model)
        {
            try
            {
                _context.Artists.Add(model);
                return await _shareService.SaveAsync();
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<Artist>?> GetAll()
        {
            try
            {
                var result = await _context.Artists
                    .Include(a => a.Events)
                    .AsNoTracking()
                    .ToListAsync();
                return result;

            }
            catch
            {
                return null;
            }
        }

        public async Task<Artist?> GetOne(Guid id)
        {
            try
            {
                var result = await _context.Artists.Include(a => a.Events).FirstOrDefaultAsync(a => a.Id == id);
                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Create(Artist model)
        {
            try
            {
                _context.Artists.Add(model);
                return await _shareService.SaveAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(Guid id, Artist model)
        {
            var result = await _context.Artists.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            result.Description = model.Description;
            result.Name = model.Name;
            result.Genre = model.Genre;
            return await _shareService.SaveAsync();
        }


        public async Task<bool> Delete(Guid id)
        {
            var result = await _context.Artists.FindAsync(id);
            if (result == null) return false;

            _context.Artists.Remove(result);
            return await _shareService.SaveAsync();
        }
    }
}
