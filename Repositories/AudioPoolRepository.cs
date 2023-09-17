
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

    private GenreDto MapGenreToDto(Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
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
            .Include(s => s.Album) 
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
	    song.DateModified = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
    
    public async Task<SongDetailsDto> CreateSongAsync(Song newSong)
    {
        newSong.DateCreated = DateTime.UtcNow;
        await _context.Songs.AddAsync(newSong);
        await _context.SaveChangesAsync();

        // Map the created song to its DTO
        var createdSongDto = new SongDetailsDto
        {
            Id = newSong.Id,
            Name = newSong.Name,
            Duration = newSong.Duration
        };    

        return createdSongDto;
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


    public async Task<IEnumerable<GenreDto>> GetAllGenres()
    {
        var genres = await _context.Genres.ToListAsync();
        if (genres == null)
        {
            return Enumerable.Empty<GenreDto>();
        }
        var genresDtos = genres.Select(g => MapGenreToDto(g));
        return genresDtos;
    }

    public async Task<GenreDetailsDto?> GetGenreByIdAsync(int id)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
        {
            return null;
        }

        var genreDetailsDto = new GenreDetailsDto
        {
            Id = genre.Id,
            Name = genre.Name,
        };
        return genreDetailsDto;
    }
    
    public async Task<GenreDetailsDto> CreateGenreAsync(Genre newGenre)
    {
        newGenre.DateCreated = DateTime.UtcNow;

        await _context.Genres.AddAsync(newGenre);
        await _context.SaveChangesAsync();

        // Map the created genre to its DTO
        var createdGenreDto = new GenreDetailsDto
        {
            Id = newGenre.Id,
            Name = newGenre.Name
        };    

        return createdGenreDto;
    }


    public async Task<Envelope<ArtistDto>> GetAllArtists(int pageNumber, int pageSize)
    {
        var artists = await _context.Artists
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        

        var artistsDtos = artists.Select(a => MapArtistToDto(a));
        
        
        artistsDtos = artistsDtos.OrderByDescending(a => a.DateOfStart);

        return new Envelope<ArtistDto>(pageNumber, pageSize, artistsDtos);
    }

    public async Task<ArtistDetailsDto?> GetArtistByIdAsync(int id)
    {
        var artist = await _context.Artists
            .FirstOrDefaultAsync(g => g.Id == id);

        if (artist == null)
        {
            return null;
        }

        var artistDetailsDto = new ArtistDetailsDto
        {
            Id = artist.Id,
            Name = artist.Name,
        };
        return artistDetailsDto;
    }

    public async Task<ArtistDetailsDto> CreateArtistAsync(Artist newArtist)
    {
        newArtist.DateCreated = DateTime.UtcNow;

        await _context.Artists.AddAsync(newArtist);
        await _context.SaveChangesAsync();

        // Map the created genre to its DTO
        var createdArtistDto = new ArtistDetailsDto
        {
            Id = newArtist.Id,
            Name = newArtist.Name
        };    

        return createdArtistDto;
    }

    public async Task UpdateArtistByIdAsync(int id, Artist updatedArtist)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null)
        {
            throw new ArgumentException($"Artist with id {id} does not exist");
        }

        artist.Name = updatedArtist.Name;
        artist.Bio = updatedArtist.Bio;
        artist.CoverImageUrl = updatedArtist.CoverImageUrl;
        artist.DateModified = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<AlbumDto>> GetAlbumsByArtistId(int id)
    {
        
        var albums = await _context.Artists
            .Include(a => a.AlbumArtists)
            .ThenInclude(aa => aa.Album)
            .Where(a => a.Id == id)
            .SelectMany(a => a.AlbumArtists)
            .Select(aa => aa.Album)
            .ToListAsync();
        
        
        if (albums == null)
        {
            return Enumerable.Empty<AlbumDto>();
        }
        var albumDtos = albums.Select(a => MapAlbumToDtoSimple(a));
        return albumDtos;
    }

}
