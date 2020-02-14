using Microsoft.EntityFrameworkCore;

namespace RecommenderService.Models
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options) { }

        public DbSet<CatalogModel> CatalogModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogModel>()
                .HasKey(o => new { o.AlbumId, o.UserId });
        }
    }
}
