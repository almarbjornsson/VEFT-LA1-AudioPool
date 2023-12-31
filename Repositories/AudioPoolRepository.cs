
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
            DateOfStart = artist.DateOfStart,
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
            Name = genre.Name,
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
        song.ModifiedBy = "AudioPoolAdmin";
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

    public async Task<Album> CreateAlbumAsync(AlbumInputModel album)
    {
        // Map the input model to an Album entity
        var albumEntity = new Album
        {
            Name = album.Name,
            ReleaseDate = album.ReleaseDate,
            CoverImageUrl = album.CoverImageUrl,
            Description = album.Description,
            DateCreated = DateTime.UtcNow,
        };

        // If artist IDs are provided, establish relationships immediately
        if (album.ArtistIds != null)
        {
            albumEntity.AlbumArtists = album.ArtistIds.Select(artistId => new AlbumArtist
            {
                // EF Core will handle the relationship once the album is saved
                AlbumsId = albumEntity.Id,
                ArtistsId = artistId
            }).ToList();
        }

        await _context.Albums.AddAsync(albumEntity);
        await _context.SaveChangesAsync();

        // Fetch the newly created album along with its associated artists
        var albumFromDb = await _context.Albums
            .Include(a => a.AlbumArtists)
            .ThenInclude(aa => aa.Artist)
            .FirstOrDefaultAsync(a => a.Id == albumEntity.Id);

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

    public async Task<ICollection<int>> GetGenreIdsByArtistId(int artistId)
    {
        return await _context.Artists
            .Where(a => a.Id == artistId)
            .SelectMany(a => a.ArtistGenres)
            .Select(ag => ag.GenresId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ArtistDto>> GetArtistsByGenre(int genreId)
    {
var artists = await _context.Genres
            .Include(g => g.Artists)
            .ThenInclude(ag => ag.Artist)
            .Where(g => g.Id == genreId)
            .SelectMany(g => g.Artists)
            .Select(ag => ag.Artist)
            .ToListAsync();
        if (artists == null)
        {
            return Enumerable.Empty<ArtistDto>();
        }
        var artistDtos = artists.Select(a => MapArtistToDto(a));
        return artistDtos;
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
            Name = newArtist.Name,
            Bio = newArtist.Bio,
            CoverImageUrl = newArtist.CoverImageUrl,
            DateOfStart = newArtist.DateOfStart,
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
        artist.ModifiedBy = "AudioPoolAdmin";
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
    
    public async Task LinkArtistToGenre(int artistId, int genreId)
    {
        // Check if the artist and genre exist
        var artist = await _context.Artists.FindAsync(artistId);
        var genre = await _context.Genres.FindAsync(genreId);

        if (artist == null)
        {
            throw new ArgumentException($"Artist with ID {artistId} was not found."); // Return false if either the artist
        }

        if (genre == null)
        {
            throw new ArgumentException($"Genre with ID {genreId} was not found."); // Return false if either the genre
        }

        if (artist.ArtistGenres == null)
        {
            artist.ArtistGenres = new List<ArtistGenre>();
        }
        
        // Add the genre to the artist's list of genres
        artist.ArtistGenres.Add(new ArtistGenre
        {
            ArtistsId = artistId,
            GenresId = genreId,
        });

        // Save changes to the database
        await _context.SaveChangesAsync();
    }

}
