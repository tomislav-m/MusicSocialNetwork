using Microsoft.EntityFrameworkCore;
using WebApi.Model.InternalModels;

namespace WebApi.Model
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext (DbContextOptions<MusicDbContext> options)
            : base (options) { }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Format> Formats { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Style> Styles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<Track>().ToTable("Track");
            modelBuilder.Entity<Format>().ToTable("Format");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<Style>().ToTable("Style");
        }
    }
}
