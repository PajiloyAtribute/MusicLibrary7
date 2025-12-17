using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicDatabase.Models;

namespace MusicDatabase.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly ApplicationDbContext _context;

        public SongRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task<List<Song>> GetAllAsync()
        {
            return await _context.Songs
                .Include(s => s.Album)
                .ToListAsync();
        }

        public async Task AddAsync(Song entity)
        {
            await _context.Songs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Song entity)
        {
            _context.Songs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var song = await GetByIdAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Songs.AnyAsync(s => s.SongId == id);
        }

        public async Task<List<Song>> GetSongsWithDetailsAsync()
        {
            return await _context.Songs
                .Include(s => s.Details)
                .Include(s => s.Album)
                .ToListAsync();
        }

        public async Task<Song?> GetSongWithArtistsAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Artists)
                .Include(s => s.Album)
                .Include(s => s.Details)
                .FirstOrDefaultAsync(s => s.SongId == id);
        }

        public async Task AddArtistToSongAsync(int songId, int artistId)
        {
            var song = await _context.Songs
                .Include(s => s.Artists)
                .FirstOrDefaultAsync(s => s.SongId == songId);

            var artist = await _context.Artists.FindAsync(artistId);

            if (song != null && artist != null)
            {
                if (!song.Artists.Any(a => a.ArtistId == artistId))
                {
                    song.Artists.Add(artist);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
