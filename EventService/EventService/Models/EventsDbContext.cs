using Microsoft.EntityFrameworkCore;

namespace EventService.Models
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventBand> Headliners { get; set; }
        public DbSet<EventBand> Supporters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventBand>()
                .HasKey(o => new { o.ArtistId, o.EventId });
        }
    }
}
