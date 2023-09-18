using Microsoft.AspNetCore.Mvc;
using Models.Entities;
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
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly IAudioPoolService _audioPoolService;

        public GenreController(IAudioPoolService audioPoolService)
        {
            _audioPoolService = audioPoolService;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _audioPoolService.GetAllGenres();

            
            var genresWithLinks = new List<GenreDto>();
            // Add hypermedia links to the genres
            foreach (var genre in genres)
            {
                genre.Links.AddReference("self", $"/api/genres/{genre.Id}");
        
                // Get artists by genre and build their links
                var artists = await _audioPoolService.GetArtistsByGenre(genre.Id);
                var artistLinks = artists.Select(a => $"/api/artists/{a.Id}").ToList();
                genre.Links.AddListReference("artists", artistLinks);
                
                genresWithLinks.Add(genre);
            }
    
            return Ok(genresWithLinks);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            // Try getting the genre
            var genre = await _audioPoolService.GetGenreByIdAsync(id);
            // If the genre is null, return a 404
            if (genre == null)
            {
                throw new ArgumentException("Genre not found.");
            }
            // Otherwise, return the genre
            
            // Add hypermedia links to the genre
            // Self
            genre.Links.AddReference("self", $"/api/genres/{id}");

            return Ok(genre);
        }

        [BasicTokenAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreInputModel genreInput)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the input model to the entity model
            var newGenre = new Genre
            {
                Name = genreInput.Name,
            };

            // Call the service to create the new song
            var createdGenre = await _audioPoolService.CreateGenreAsync(newGenre);

            // Return the created song along with a 201 Created status code
            return CreatedAtAction(nameof(GetGenreById), new { id = createdGenre.Id }, createdGenre);
        }
        
    }
}