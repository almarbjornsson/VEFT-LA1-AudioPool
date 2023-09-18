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
    [Route("api/artist")]
    public class ArtistController : ControllerBase
    {
        private readonly IAudioPoolService _audioPoolService;

        public ArtistController(IAudioPoolService audioPoolService)
        {
            _audioPoolService = audioPoolService;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllArtists(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25
            )
        {
            var artists = await _audioPoolService.GetAllArtists(pageNumber, pageSize);

            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            // Try getting the artist
            var artist = await _audioPoolService.GetArtistByIdAsync(id);
            // If the artist is null, return a 404
            if (artist == null)
            {
                return NotFound();
            }
            // Otherwise, return the artist
            
            // Add hypermedia links to the artist
            
            // add a link to the artist's albums
            artist.Links.AddListReference("albums", new List<string> { $"/api/artists/{id}/albums" });
            // Self
            artist.Links.AddReference("self", $"/api/artists/{id}");
            // edit
            artist.Links.AddReference("edit", $"/api/artists/{id}");

            return Ok(artist);
        }

        [BasicTokenAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] ArtistInputModel artistInput)
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
                Bio = artistInput.Bio,
                CoverImageUrl = artistInput.CoverImageUrl,
                DateOfStart = artistInput.DateOfStart,
            };

            // Call the service to create the new song
            var createdArtist = await _audioPoolService.CreateArtistAsync(newArtist);

            // Return the created song along with a 201 Created status code
            return CreatedAtAction(nameof(GetArtistById), new { id = createdArtist.Id }, createdArtist);
        }
        
        [BasicTokenAuthorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(int id, [FromBody] ArtistInputModel artistInput)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException("Invalid input.");
            }

            var existingArtist = await _audioPoolService.GetArtistByIdAsync(id);
            if (existingArtist == null)
            {
                throw new ArgumentException("Artist not found.");
            }

            var updatedArtist = new Artist
            {
                Id = id,
                Name = artistInput.Name,
                Bio = artistInput.Bio,
                CoverImageUrl = artistInput.CoverImageUrl,
            };

            await _audioPoolService.UpdateArtistByIdAsync(id, updatedArtist);
            return NoContent();
        }

        [HttpGet("{id}/albums")]
        public async Task<IActionResult> GetAlbumsByArtistId(int id)
        {
            var albums = await _audioPoolService.GetAlbumsByArtistId(id);
        
            List<AlbumDto> albumDtosWithLinks = new List<AlbumDto>();
            
            // For each song, add hypermedia links
            foreach (var album in albums)
            {
                // Self
                album.Links.AddReference("self", $"/api/albums/{album.Id}");
                // delete
                album.Links.AddReference("delete", $"/api/albums/{album.Id}");
                // album
                album.Links.AddReference("album", $"/api/artist/{id}");
            
                albumDtosWithLinks.Add(album);
            }
            
        
            return Ok(albumDtosWithLinks);
        }
        
        [HttpPost("{artistId}/genres/{genreId}")]
        public async Task<IActionResult> LinkArtistToGenre(int artistId, int genreId)
        {
            await _audioPoolService.LinkArtistToGenre(artistId, genreId);
            return NoContent(); // Return a success response
        }
        
        
    }
}