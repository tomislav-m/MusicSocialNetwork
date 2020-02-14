using Microsoft.EntityFrameworkCore;

namespace RecommenderService.Models
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions<MusicDbContext> options)
            : base(options) { }

        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().ToTable("Album");
        }
    }
}
