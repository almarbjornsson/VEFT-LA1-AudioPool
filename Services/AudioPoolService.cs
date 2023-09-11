using Common.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services;
public class AudioPoolService : IAudioPoolService
{
    private readonly IAudioPoolRepository _repository;

    public AudioPoolService(IAudioPoolRepository repository)
    {
        _repository = repository;
    }

    // Album Operations

    public async Task<List<Album>> GetAllAlbumsAsync()
    {
        return await _repository.GetAllAlbumsAsync();
    }

    public async Task<Album> GetAlbumByIdAsync(int id)
    {
        return await _repository.GetAlbumByIdAsync(id);
    }

    public async Task AddAlbumAsync(Album album)
    {
        await _repository.AddAlbumAsync(album);
    }

    public async Task UpdateAlbumAsync(Album album)
    {
        await _repository.UpdateAlbumAsync(album);
    }

    public async Task DeleteAlbumAsync(int id)
    {
        await _repository.DeleteAlbumAsync(id);
    }

    // Artist Operations

    public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await _repository.GetAllArtistsAsync();
    }

    public async Task<Artist> GetArtistByIdAsync(int id)
    {
        return await _repository.GetArtistByIdAsync(id);
    }

    public async Task AddArtistAsync(Artist artist)
    {
        await _repository.AddArtistAsync(artist);
    }

    public async Task UpdateArtistAsync(Artist artist)
    {
        await _repository.UpdateArtistAsync(artist);
    }

    public async Task DeleteArtistAsync(int id)
    {
        await _repository.DeleteArtistAsync(id);
    }

    // Genre Operations

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _repository.GetAllGenresAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(int id)
    {
        return await _repository.GetGenreByIdAsync(id);
    }

    public async Task AddGenreAsync(Genre genre)
    {
        await _repository.AddGenreAsync(genre);
    }

    public async Task UpdateGenreAsync(Genre genre)
    {
        await _repository.UpdateGenreAsync(genre);
    }

    public async Task DeleteGenreAsync(int id)
    {
        await _repository.DeleteGenreAsync(id);
    }

    // Song Operations

    public async Task<List<Song>> GetAllSongsAsync()
    {
        return await _repository.GetAllSongsAsync();
    }

    public async Task<Song> GetSongByIdAsync(int id)
    {
        return await _repository.GetSongByIdAsync(id);
    }

    public async Task AddSongAsync(Song song)
    {
        await _repository.AddSongAsync(song);
    }

    public async Task UpdateSongAsync(Song song)
    {
        await _repository.UpdateSongAsync(song);
    }

    public async Task DeleteSongAsync(int id)
    {
        await _repository.DeleteSongAsync(id);
    }
}
