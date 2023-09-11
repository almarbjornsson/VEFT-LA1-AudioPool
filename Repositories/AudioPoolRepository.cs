
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Repositories;

namespace AudioPool.Models;

public class AudioPoolRepository : IAudioPoolRepository
{
    private readonly AudioPoolDbContext _context;

    private ArtistDto MapArtistToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Bio = artist.Bio,
            CoverImageUrl = artist.CoverImageUrl,
            // artist.DateStarted is a datetime string
            DateOfStart = DateTime.Parse(artist.DateStarted)
        };
    }
    private SongDto MapSongToDto(Song song)
    {
        return new SongDto
        {
            Id = song.Id,
            Name = song.Name,
            Duration = song.Duration,
        };
    }
    
    private AlbumDetailsDto MapAlbumToDto(Album album)
    {
        return new AlbumDetailsDto
        {
            Id = album.Id,
            Name = album.Name,
            ReleaseDate = album.ReleaseDate,
            CoverImageUrl = album.CoverImageUrl,
            Artists = album.Artists?.Select(a => MapArtistToDto(a)) ?? Enumerable.Empty<ArtistDto>(),
            Songs = album.Songs?.Select(s => MapSongToDto(s)) ?? Enumerable.Empty<SongDto>(),
        };
    }
    
    
    
    public AudioPoolRepository(AudioPoolDbContext context)
    {
        _context = context;
    }


    public async Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        
        if (album == null)
        {
            return null;
        }
        
        var albumDto = MapAlbumToDto(album);
        return albumDto;
    }

    public async Task<IEnumerable<SongDto>> GetSongsByAlbumId(int id)
    {
        var songs = await _context.Songs.Where(s => s.AlbumId == id).ToListAsync();
        if (songs == null)
        {
            return Enumerable.Empty<SongDto>();
        }
        var songDtos = songs.Select(s => MapSongToDto(s));
        return songDtos;
    }
}
