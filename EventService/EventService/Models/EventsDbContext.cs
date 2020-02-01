using Microsoft.EntityFrameworkCore;

namespace EventService.Models
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventBandHealiner> Headliners { get; set; }
        public DbSet<EventBandSupporter> Supporters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventBandHealiner>()
                .HasKey(o => new { o.ArtistId, o.EventId });
            modelBuilder.Entity<EventBandSupporter>()
                .HasKey(o => new { o.ArtistId, o.EventId });
        }
    }
}
