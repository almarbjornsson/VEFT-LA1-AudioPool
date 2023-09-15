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
    [Route("api/artist")]
    public class ArtistController : ControllerBase
    {
        private readonly IAudioPoolService _audioPoolService;

        public ArtistController(IAudioPoolService audioPoolService)
        {
            _audioPoolService = audioPoolService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            // Try getting the genre
            var artist = await _audioPoolService.GetArtistByIdAsync(id);
            // If the genre is null, return a 404
            if (artist == null)
            {
                return NotFound();
            }
            // Otherwise, return the genre
            
            // TODO: Add hypermedia links to the genre

            return Ok(artist);
        }

        // post 
        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] GenreInputModel artistInput)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the input model to the entity model
            var newArtist = new Artist
            {
                Name = artistInput.Name,
            };

            // Call the service to create the new song
            var createdArtist = await _audioPoolService.CreateArtistAsync(newArtist);

            // Return the created song along with a 201 Created status code
            return CreatedAtAction(nameof(GetArtistById), new { id = createdArtist.Id }, createdArtist);
        }
        
    }
}