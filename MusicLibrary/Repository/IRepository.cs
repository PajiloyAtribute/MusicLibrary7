using System.Collections.Generic;
using System.Threading.Tasks;
using MusicDatabase.Models;

namespace MusicDatabase.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public interface IArtistRepository : IRepository<Artist>
    {
        Task<List<Artist>> GetArtistsWithAlbumsAsync();
        Task<Artist?> GetArtistWithSongsAsync(int id);
        Task<List<Artist>> GetPagedAsync(int page, int pageSize, string? search);
    }

    public interface IAlbumRepository : IRepository<Album>
    {
        Task<List<Album>> GetAlbumsWithSongsAsync();
        Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId);
    }

    public interface ISongRepository : IRepository<Song>
    {
        Task<List<Song>> GetSongsWithDetailsAsync();
        Task<Song?> GetSongWithArtistsAsync(int id);
        Task AddArtistToSongAsync(int songId, int artistId);
    }
}
