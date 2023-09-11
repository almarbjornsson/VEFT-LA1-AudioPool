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

    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
    {
        return Ok(await _audioPoolService.GetAllAlbumsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlbumById(int id)
    {
        return Ok(await _audioPoolService.GetAlbumByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum([FromBody] Album album)
    {
        await _audioPoolService.AddAlbumAsync(album);
        return CreatedAtAction(nameof(GetAlbumById), new { id = album.Id }, album);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlbum(int id, [FromBody] Album album)
    {
        if (id != album.Id) return BadRequest("ID mismatch.");
        await _audioPoolService.UpdateAlbumAsync(album);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        await _audioPoolService.DeleteAlbumAsync(id);
        return NoContent();
    }
}
