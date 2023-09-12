using Models.DTOs;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces;
public interface IAudioPoolRepository
{
    Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id);
    
    Task DeleteAlbumByIdAsync(int id);
    
    Task<Album> CreateAlbumAsync(Album album);
    Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id);
    Task<SongDetailsDto?> GetSongByIdAsync(int id);

}
