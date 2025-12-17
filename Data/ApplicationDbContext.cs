using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicDatabase.Models;

namespace MusicLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongDetails> SongDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Albums)
                .WithOne(a => a.Artist)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Songs)
                .WithOne(s => s.Album)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Song>()
                .HasOne(s => s.Details)
                .WithOne(sd => sd.Song)
                .HasForeignKey<SongDetails>(sd => sd.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Songs)
                .WithMany(s => s.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistSong",
                    right => right.HasOne<Song>().WithMany().HasForeignKey("SongsSongId").OnDelete(DeleteBehavior.Restrict),
                    left => left.HasOne<Artist>().WithMany().HasForeignKey("ArtistsArtistId").OnDelete(DeleteBehavior.Restrict)
                )
                .HasKey("ArtistsArtistId", "SongsSongId");
        }
    }
}
