using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Threading.Tasks;
using AudioPool.Models;
using Common.Interfaces;
using Models.DTOs;

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
        // Add hypermedia links to the album
        
        // add a link to the album's songs
        album.Links.AddListReference("songs", new List<string> { $"/api/albums/{id}/songs" });
        // Self
        album.Links.AddReference("self", $"/api/albums/{id}");
        // delete
        album.Links.AddReference("delete", $"/api/albums/{id}");
        // Artists
        album.Links.AddListReference("artists", album.Artists.Select(a => $"/api/artists/{a.Id}"));
        
        
        // Add hypermedia links to each song in the album
        album.Songs = album.Songs.Select(s =>
        {
            // Self
            s.Links.AddReference("self", $"/api/songs/{s.Id}");
            return s;
        });
        return Ok(album);
    }

    [HttpGet("{id}/songs")]
    public async Task<IActionResult> GetSongsInAlbum(int id)
    {
        var songs = await _audioPoolService.GetSongsByAlbumId(id);
        
        List<SongDto> songDtosWithLinks = new List<SongDto>();
        
        // For each song, add hypermedia links
        foreach (var song in songs)
        {
            // Self
            song.Links.AddReference("self", $"/api/songs/{song.Id}");
            // delete
            song.Links.AddReference("delete", $"/api/songs/{song.Id}");
            // edit
            song.Links.AddReference("edit", $"/api/songs/{song.Id}");
            // album
            song.Links.AddReference("album", $"/api/albums/{id}");
            
            songDtosWithLinks.Add(song);
        }
        
        return Ok(songDtosWithLinks);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _audioPoolService.GetAlbumByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }
        await _audioPoolService.DeleteAlbumByIdAsync(id);
        return NoContent();
    }

}
