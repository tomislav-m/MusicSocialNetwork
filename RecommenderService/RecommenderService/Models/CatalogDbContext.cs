using Microsoft.EntityFrameworkCore;

namespace RecommenderService.Models
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options) { }

        public DbSet<CatalogModel> CatalogModels;
    }
}
