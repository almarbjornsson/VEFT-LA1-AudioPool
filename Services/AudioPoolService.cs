using Common.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioPool.Models;
using Mapster;
using Models.DTOs;
using Models.InputModels;

namespace Services;
public class AudioPoolService : IAudioPoolService
{
    private readonly IAudioPoolRepository _repository;

    public AudioPoolService(IAudioPoolRepository repository)
    {
        _repository = repository;
    }


    public async Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id)
    {
        return await _repository.GetAlbumByIdAsync(id);
    }

    public async Task DeleteAlbumByIdAsync(int id)
    {
        await _repository.DeleteAlbumByIdAsync(id);
    }

    public async Task<AlbumDetailsDto> CreateAlbumAsync(AlbumInputModel albumInputModel)
    {
        
        // Create the album - this should return the album from the database
        var albumFromDb = await _repository.CreateAlbumAsync(albumInputModel);
        
        
        // Map the album from the database to a DTO
        var albumDto = new AlbumDetailsDto
        {
            Name = albumFromDb.Name,
            ReleaseDate = albumFromDb.ReleaseDate,
            CoverImageUrl = albumFromDb.CoverImageUrl,
            Description = albumFromDb.Description,
            Artists = albumFromDb.AlbumArtists.Select(aa => aa.Artist.Adapt<ArtistDto>()),
        };
        
        
        return albumDto;
    }

    public async Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id)
    {
        return await _repository.GetSongsByAlbumId(id);
    }
    public async Task<SongDetailsDto?> GetSongByIdAsync(int id)
    {
        return await _repository.GetSongByIdAsync(id);
    }
    
    public async Task DeleteSongByIdAsync(int id)
    {
        await _repository.DeleteSongByIdAsync(id);
    }
    
    public async Task UpdateSongByIdAsync(int id, Song updatedSong)
    {
        await _repository.UpdateSongByIdAsync(id, updatedSong);
    }

    public async Task<SongDetailsDto> CreateSongAsync(Song newSong)
    {
        // Call the repository to create the new song and get the DTO
        var createdSongDto = await _repository.CreateSongAsync(newSong);

        return createdSongDto;
    }
    
    public async Task<IEnumerable<GenreDto>> GetAllGenres()
    {
        return await _repository.GetAllGenres();
    }
    
    public async Task<GenreDetailsDto?> GetGenreByIdAsync(int id)
    {
        return await _repository.GetGenreByIdAsync(id);
    }

    public async Task<GenreDetailsDto> CreateGenreAsync(Genre newGenre)
    {
        // Call the repository to create the new genre and get the DTO
        var createdGenreDto = await _repository.CreateGenreAsync(newGenre);

        return createdGenreDto;
    }
    
    public async Task<Envelope<ArtistDto>> GetAllArtists(int pageNumber, int pageSize)
    {
        return await _repository.GetAllArtists(pageNumber, pageSize);
    }

    public async Task<ArtistDetailsDto?> GetArtistByIdAsync(int id)
    {
        return await _repository.GetArtistByIdAsync(id);
    }

    public async Task<ArtistDetailsDto> CreateArtistAsync(Artist newArtist)
    {
        // Call the repository to create the new genre and get the DTO
        var createdArtistDto = await _repository.CreateArtistAsync(newArtist);

        return createdArtistDto;
    }
    
    public async Task UpdateArtistByIdAsync(int id, Artist updatedArtist)
    {
        await _repository.UpdateArtistByIdAsync(id, updatedArtist);
    }

    public async Task<IEnumerable<AlbumDto>> GetAlbumsByArtistId(int id)
    {
        return await _repository.GetAlbumsByArtistId(id);
    }

    public async Task LinkArtistToGenre(int artistId, int genreId)
    {
        await _repository.LinkArtistToGenre(artistId, genreId);
    }
    
}

