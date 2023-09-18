using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioPool.Models;
using Models.InputModels;
using Models.Entities;
using Models;

namespace Common.Interfaces;
public interface IAudioPoolService
{
    Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id);
    
    Task DeleteAlbumByIdAsync(int id);
    
    Task<AlbumDetailsDto> CreateAlbumAsync(AlbumInputModel albumInputModel);
    
    Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id);
    
    Task<SongDetailsDto?> GetSongByIdAsync(int id);
    
    Task DeleteSongByIdAsync(int id);
    
    Task UpdateSongByIdAsync(int id, Song updatedSong);
    
    Task<SongDetailsDto> CreateSongAsync(Song newSong);

    Task<IEnumerable<GenreDto>> GetAllGenres();
    
    Task<ICollection<int> > GetGenreIdsByArtistId(int artistId);

    Task<GenreDetailsDto?> GetGenreByIdAsync(int id);
    
    Task<GenreDetailsDto> CreateGenreAsync(Genre newGenre);

    Task<Envelope<ArtistDto>> GetAllArtists(int pageNumber, int pageSize);

    /// <summary>
    /// Helper for hypermedia links
    /// </summary>
    /// <param name="genreId"></param>
    /// <returns></returns>
    Task<IEnumerable<ArtistDto>> GetArtistsByGenre(int genreId);

    
    Task<ArtistDetailsDto?> GetArtistByIdAsync(int id);
    
    Task<ArtistDetailsDto> CreateArtistAsync(Artist newArtist);

    Task UpdateArtistByIdAsync(int id, Artist updatedArtist);
    
    Task<IEnumerable<AlbumDto>> GetAlbumsByArtistId(int id);
    
    Task LinkArtistToGenre(int artistId, int genreId);

}
