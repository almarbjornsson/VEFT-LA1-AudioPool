using Common.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs;

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
