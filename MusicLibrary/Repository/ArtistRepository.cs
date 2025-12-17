using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicDatabase.Models;

namespace MusicDatabase.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Artist?> GetByIdAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<List<Artist>> GetAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task AddAsync(Artist entity)
        {
            await _context.Artists.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Artist entity)
        {
            _context.Artists.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await GetByIdAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artists.AnyAsync(a => a.ArtistId == id);
        }

        public async Task<List<Artist>> GetArtistsWithAlbumsAsync()
        {
            return await _context.Artists
                .Include(a => a.Albums)
                .ToListAsync();
        }

        public async Task<Artist?> GetArtistWithSongsAsync(int id)
        {
            return await _context.Artists
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.ArtistId == id);
        }

        public async Task<List<Artist>> GetPagedAsync(int page, int pageSize, string? search)
        {
            var query = _context.Artists.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => a.Name.Contains(search));

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
