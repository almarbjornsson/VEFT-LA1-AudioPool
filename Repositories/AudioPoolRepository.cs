
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Models.InputModels;
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
            Artists = album.AlbumArtists?.Select(a => MapArtistToDto(a.Artist)) ?? Enumerable.Empty<ArtistDto>(),
            Songs = album.Songs?.Select(s => MapSongToDto(s)) ?? Enumerable.Empty<SongDto>(),
        };
    }
    
    private AlbumDto MapAlbumToDtoSimple(Album album)
    {
        return new AlbumDto
        {
            Id = album.Id,
            Name = album.Name,
            ReleaseDate = album.ReleaseDate,
            CoverImageUrl = album.CoverImageUrl,
            Description = album.Description,
        };
    }


    public AudioPoolRepository(AudioPoolDbContext context)
    {
        _context = context;
    }


    public async Task<SongDetailsDto?> GetSongByIdAsync(int id)
    {
        var song = await _context.Songs
            .Include(s => s.Album) // Eagerly load Album data
            .FirstOrDefaultAsync(s => s.Id == id);

        if (song == null)
        {
            return null;
        }

        var songDetailsDto = new SongDetailsDto
        {
            Id = song.Id,
            Name = song.Name,
            Duration = song.Duration,
            Album = MapAlbumToDtoSimple(song.Album)
        };
        return songDetailsDto;
    }
    
    public async Task DeleteSongByIdAsync(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            throw new ArgumentException($"Song with id {id} does not exist");
        }
        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateSongByIdAsync(int id, Song updatedSong)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            throw new ArgumentException($"Song with id {id} does not exist");
        }

        song.Name = updatedSong.Name;
        song.Duration = updatedSong.Duration;
        song.AlbumId = updatedSong.AlbumId;

        await _context.SaveChangesAsync();
    }


    public async Task<AlbumDetailsDto?> GetAlbumByIdAsync(int id)
    {
        var album = await _context.Albums
            .Include(a => a.AlbumArtists)
            .ThenInclude(aa => aa.Artist) // Eagerly load Artist data
            .Include(s => s.Songs) // Join Songs
            .FirstOrDefaultAsync(a => a.Id == id);
         
        if (album == null)
        {
            return null;
        }
        
        var albumDto = MapAlbumToDto(album);
        return albumDto;
    }

    public async Task DeleteAlbumByIdAsync(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null)
        {
            throw new ArgumentException($"Album with id {id} does not exist");
        }
        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
    }

    public async Task<Album> CreateAlbumAsync(Album album)
    {
        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();
        
        // Sanity check - make sure the album was actually added by fetching it again
        var albumFromDb = await _context.Albums.FindAsync(album.Id);
        if (albumFromDb == null)
        {
            throw new Exception("Album was not added to the database");
        }
        return albumFromDb;
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
