using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System;
using System.Threading.Tasks;
using AudioPool.Models;
using Common.Interfaces;
using Models.DTOs;
using Models.InputModels;
using System.Collections.Generic;
using WebAPI.Attributes;

namespace WebAPI.Controllers
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
            var song = await _audioPoolService.GetSongByIdAsync(id);
            if (song == null)
            {
                throw new ArgumentException("Song not found.");
            }

            // Add hypermedia links to the song
            song.Links.AddReference("self", $"/api/songs/{id}");
            song.Links.AddReference("delete", $"/api/songs/{id}");
            song.Links.AddReference("edit", $"/api/songs/{id}");
            song.Links.AddReference("album", $"/api/albums/{song.Album.Id}");

            return Ok(song);
        }

        [BasicTokenAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _audioPoolService.GetSongByIdAsync(id);
            if (song == null)
            {
                throw new ArgumentException("Song not found.");
            }
            await _audioPoolService.DeleteSongByIdAsync(id);
            return NoContent();
        }

        [BasicTokenAuthorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongInputModel songInput)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException("Invalid input.");
            }

            var existingSong = await _audioPoolService.GetSongByIdAsync(id);
            if (existingSong == null)
            {
                throw new ArgumentException("Song not found.");
            }

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

        [BasicTokenAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] SongInputModel songInput)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException("Invalid input.");
            }

            var album = await _audioPoolService.GetAlbumByIdAsync(songInput.AlbumId);
            if (album == null)
            {
                throw new ArgumentException("Invalid AlbumId.");
            }

            var newSong = new Song
            {
                Name = songInput.Name,
                Duration = songInput.Duration,
                AlbumId = songInput.AlbumId
            };

            var createdSong = await _audioPoolService.CreateSongAsync(newSong);
            return CreatedAtAction(nameof(GetSongById), new { id = createdSong.Id }, createdSong);
        }
    }
}
