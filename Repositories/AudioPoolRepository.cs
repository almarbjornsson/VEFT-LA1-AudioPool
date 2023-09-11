
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repositories;

namespace AudioPool.Models;

public class AudioPoolRepository : IAudioPoolRepository
{
    private readonly AudioPoolDbContext _context;

    public AudioPoolRepository(AudioPoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Album>> GetAllAlbumsAsync()
    {
        return await _context.Albums.ToListAsync();
    }

    public async Task<Album> GetAlbumByIdAsync(int id)
    {
        return await _context.Albums.FindAsync(id);
    }

    public async Task AddAlbumAsync(Album album)
    {
        await _context.Albums.AddAsync(album);
        await SaveChangesAsync();
    }

    public async Task UpdateAlbumAsync(Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAlbumAsync(int id)
    {
        var album = await GetAlbumByIdAsync(id);
        if (album != null)
        {
            _context.Albums.Remove(album);
            await SaveChangesAsync();
        }
    }

    public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await _context.Artists.ToListAsync();
    }

    public async Task<Artist> GetArtistByIdAsync(int id)
    {
        return await _context.Artists.FindAsync(id);
    }

    public async Task AddArtistAsync(Artist artist)
    {
        await _context.Artists.AddAsync(artist);
        await SaveChangesAsync();
    }

    public async Task UpdateArtistAsync(Artist artist)
    {
        _context.Entry(artist).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteArtistAsync(int id)
    {
        var artist = await GetArtistByIdAsync(id);
        if (artist != null)
        {
            _context.Artists.Remove(artist);
            await SaveChangesAsync();
        }
    }

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task AddGenreAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await SaveChangesAsync();
    }

    public async Task UpdateGenreAsync(Genre genre)
    {
        _context.Entry(genre).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(int id)
    {
        var genre = await GetGenreByIdAsync(id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
            await SaveChangesAsync();
        }
    }

    public async Task<List<Song>> GetAllSongsAsync()
    {
        return await _context.Songs.ToListAsync();
    }

    public async Task<Song> GetSongByIdAsync(int id)
    {
        return await _context.Songs.FindAsync(id);
    }

    public async Task AddSongAsync(Song song)
    {
        await _context.Songs.AddAsync(song);
        await SaveChangesAsync();
    }

    public async Task UpdateSongAsync(Song song)
    {
        _context.Entry(song).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteSongAsync(int id)
    {
        // Fetch the song entity by its ID
        var song = await GetSongByIdAsync(id);

        // Ensure the entity exists before attempting to delete it
        if (song != null)
        {
            // Remove the entity from the context
            _context.Songs.Remove(song);

            // Commit changes to the database
            await SaveChangesAsync();
        }
    }
    // Method to save changes and handle exceptions
    private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Handle exception here, possibly logging the error and/or re-throwing
            throw new Exception("An error occurred while saving changes", ex);
        }
    }
}
