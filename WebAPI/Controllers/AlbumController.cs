using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Presentation.Controllers;
[ApiController]
[Route("api/albums")]
public class AlbumController : ControllerBase
{
    private readonly IAudioPoolService _audioPoolService;

    public AlbumController(IAudioPoolService audioPoolService)
    {
        _audioPoolService = audioPoolService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlbumById(int id)
    {
        // Try getting the album
        var album = await _audioPoolService.GetAlbumByIdAsync(id);
        // If the album is null, return a 404
        if (album == null)
        {
            return NotFound();
        }
        // Otherwise, return the album
        return Ok(album);
    }

    [HttpGet("{id}/songs")]
    public async Task<IActionResult> GetSongsInAlbum(int id)
    {
        return Ok(await _audioPoolService.GetSongsByAlbumId(id));
    }


}
