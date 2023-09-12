using Common.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        // Map the input model to an album
        var album = new Album
        {
            Name = albumInputModel.Name,
            ReleaseDate = albumInputModel.ReleaseDate,
            CoverImageUrl = albumInputModel.CoverImageUrl,
            Description = albumInputModel.Description,
            AlbumArtists = albumInputModel.ArtistIds.Select(a => new AlbumArtist
            {
                ArtistsId = a
            }).ToList(),
        };
        // Create the album - this should return the album from the database
        var albumFromDb = await _repository.CreateAlbumAsync(album);
        
        // Map the album from the database to a DTO
        var albumDto = new AlbumDetailsDto
        {
            Id = albumFromDb.Id,
            Name = albumFromDb.Name,
            ReleaseDate = albumFromDb.ReleaseDate,
            CoverImageUrl = albumFromDb.CoverImageUrl,
            Artists = albumFromDb.AlbumArtists?.Select(a => new ArtistDto
            {
                Id = a.ArtistsId,
                Name = a.Artist.Name,
                Bio = a.Artist.Bio,
                CoverImageUrl = a.Artist.CoverImageUrl,
                DateOfStart = DateTime.Parse(a.Artist.DateStarted)
            }) ?? Enumerable.Empty<ArtistDto>(),
            Songs = albumFromDb.Songs?.Select(s => new SongDto
            {
                Id = s.Id,
                Name = s.Name,
                Duration = s.Duration,
            }) ?? Enumerable.Empty<SongDto>(),
        };
        return albumDto;
    }

    public async Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id)
    {
        return await _repository.GetSongsByAlbumId(id);
    }
    // AudioPoolService.cs
    public async Task<SongDetailsDto?> GetSongByIdAsync(int id)
    {
        return await _repository.GetSongByIdAsync(id);
    }

    
    
    
}
