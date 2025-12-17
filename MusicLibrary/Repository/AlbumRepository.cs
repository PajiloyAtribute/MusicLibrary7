using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicDatabase.Models;

namespace MusicDatabase.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDbContext _context;

        public AlbumRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Album?> GetByIdAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.AlbumId == id);
        }

        public async Task<List<Album>> GetAllAsync()
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task AddAsync(Album entity)
        {
            await _context.Albums.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Album entity)
        {
            _context.Albums.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var album = await GetByIdAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Albums.AnyAsync(a => a.AlbumId == id);
        }

        public async Task<List<Album>> GetAlbumsWithSongsAsync()
        {
            return await _context.Albums
                .Include(a => a.Songs)
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId)
        {
            return await _context.Albums
                .Where(a => a.ArtistId == artistId)
                .Include(a => a.Songs)
                .ToListAsync();
        }
    }
}
