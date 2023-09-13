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
        
        // post 
        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] SongInputModel songInput)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the input model to the entity model
            var newSong = new Song
            {
                Name = songInput.Name,
                Duration = songInput.Duration,
                AlbumId = songInput.AlbumId
            };

            // Call the service to create the new song
            var createdSong = await _audioPoolService.CreateSongAsync(newSong);

            // Return the created song along with a 201 Created status code
            return CreatedAtAction(nameof(GetSongById), new { id = createdSong.Id }, createdSong);
        }
        
    }
}