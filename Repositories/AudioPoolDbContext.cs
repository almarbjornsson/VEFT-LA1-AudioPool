
using Microsoft.EntityFrameworkCore;

using Models.Entities;

namespace Repositories

{
    public class AudioPoolDbContext : DbContext
    {
        public AudioPoolDbContext(DbContextOptions<AudioPoolDbContext> options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}