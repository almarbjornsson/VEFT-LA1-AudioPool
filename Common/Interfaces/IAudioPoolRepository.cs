using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces;
public interface IAudioPoolRepository
{
    Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id);
    
    Task DeleteAlbumByIdAsync(int id);
    Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id);
}
