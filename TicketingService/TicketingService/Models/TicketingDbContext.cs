using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingService.Models
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets;

        public DbSet<EventTicketsInfo> TicketsInfo;
    }
}
