using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketingService.Models;

namespace TicketingService.Services
{
    public interface ITicketsService
    {
        Task<Ticket> BuyTicket(int userId, int eventId, int count = 1);
        Task<EventTicketsInfo> GetEventTicketsInfo(int eventId);
    }

    public class TicketsService : ITicketsService
    {
        private readonly TicketingDbContext _context;

        public TicketsService(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> BuyTicket(int userId, int eventId, int count = 1)
        {
            var ticket = new Ticket { UserId = userId, EventId = eventId, Count = count };

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<EventTicketsInfo> GetEventTicketsInfo(int eventId)
        {
            var info = await _context.TicketsInfo.SingleOrDefaultAsync(x => x.EventId == eventId);

            return info;
        }
    }
}
