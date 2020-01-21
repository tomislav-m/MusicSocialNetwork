using Microsoft.EntityFrameworkCore;

namespace CatalogService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AlbumRating> AlbumRatings { get; set; }
        public DbSet<UserAlbum> UserAlbums { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
