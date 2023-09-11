using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces;
public interface IAudioPoolService
{
    Task<List<Album>> GetAllAlbumsAsync();
    Task<Album> GetAlbumByIdAsync(int id);
    Task AddAlbumAsync(Album album);
    Task UpdateAlbumAsync(Album album);
    Task DeleteAlbumAsync(int id);

    Task<List<Artist>> GetAllArtistsAsync();
    Task<Artist> GetArtistByIdAsync(int id);
    Task AddArtistAsync(Artist artist);
    Task UpdateArtistAsync(Artist artist);
    Task DeleteArtistAsync(int id);

    Task<List<Genre>> GetAllGenresAsync();
    Task<Genre> GetGenreByIdAsync(int id);
    Task AddGenreAsync(Genre genre);
    Task UpdateGenreAsync(Genre genre);
    Task DeleteGenreAsync(int id);

    Task<List<Song>> GetAllSongsAsync();
    Task<Song> GetSongByIdAsync(int id);
    Task AddSongAsync(Song song);
    Task UpdateSongAsync(Song song);
    Task DeleteSongAsync(int id);
}
