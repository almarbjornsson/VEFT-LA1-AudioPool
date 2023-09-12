using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Threading.Tasks;
using AudioPool.Models;
using Common.Interfaces;
using Models.DTOs;
using Models.InputModels;
using System.Collections.Generic;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/songs")]
    public class SongsController : ControllerBase
    {
        private readonly IAudioPoolService _audioPoolService;

        public SongsController(IAudioPoolService audioPoolService)
        {
            _audioPoolService = audioPoolService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSongById(int id)
        {
            // Try getting the song
            var song = await _audioPoolService.GetSongByIdAsync(id);
            // If the song is null, return a 404
            if (song == null)
            {
                return NotFound();
            }
            // Otherwise, return the song
            
            // Add hypermedia links to the song
            song.Links.AddReference("self", $"/api/songs/{id}");
            song.Links.AddReference("delete", $"/api/songs/{id}");
            song.Links.AddReference("edit", $"/api/songs/{id}");
            song.Links.AddReference("album", $"/api/albums/{song.Album.Id}");

            return Ok(song);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _audioPoolService.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            await _audioPoolService.DeleteSongByIdAsync(id);
            return NoContent();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongInputModel songInput)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSong = await _audioPoolService.GetSongByIdAsync(id);
            if (existingSong == null)
            {
                return NotFound();
            }

            // Map the InputModel to the entity model
            var updatedSong = new Song
            {
                Id = id, 
                Name = songInput.Name,
                Duration = songInput.Duration,
                AlbumId = songInput.AlbumId 
            };

            await _audioPoolService.UpdateSongByIdAsync(id, updatedSong);

            return NoContent();
        }


    }
}