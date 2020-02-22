using Microsoft.EntityFrameworkCore;

namespace TicketingService.Models
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<EventTicketsInfo> TicketsInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasKey(o => new { o.EventId, o.UserId, o.DateTimeBought });
            modelBuilder.Entity<EventTicketsInfo>()
                .HasKey(o => o.EventId);
        }
    }
}
