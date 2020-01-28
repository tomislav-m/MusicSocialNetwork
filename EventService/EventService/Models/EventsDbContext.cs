using Microsoft.EntityFrameworkCore;

namespace EventService.Models
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Headliner> Headliners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Headliner>()
                .HasKey(o => new { o.ArtistId, o.EventId });
        }
    }
}
