using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Repositories
{
    public class AudioPoolDbContext : DbContext
    {
        // Constructor
        public AudioPoolDbContext(DbContextOptions<AudioPoolDbContext> options) : base(options)
        {
        }

        // Model configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many: AlbumArtist
            modelBuilder.Entity<AlbumArtist>()
                .HasKey(aa => new { aa.AlbumsId, aa.ArtistsId });

            modelBuilder.Entity<AlbumArtist>()
                .HasOne(aa => aa.Album)
                .WithMany(a => a.AlbumArtists)
                .HasForeignKey(aa => aa.AlbumsId);

            modelBuilder.Entity<AlbumArtist>()
                .HasOne(aa => aa.Artist)
                .WithMany(a => a.AlbumArtists)
                .HasForeignKey(aa => aa.ArtistsId);

            // Many-to-Many: ArtistGenre
            modelBuilder.Entity<ArtistGenre>()
                .HasKey(ag => new { ag.ArtistsId, ag.GenresId });

            modelBuilder.Entity<ArtistGenre>()
                .HasOne(ag => ag.Artist)
                .WithMany(a => a.ArtistGenres)
                .HasForeignKey(ag => ag.ArtistsId);

            modelBuilder.Entity<ArtistGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(g => g.Artists)
                .HasForeignKey(ag => ag.GenresId);
        }

        // DbSets
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}