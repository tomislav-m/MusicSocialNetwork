using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketingService.Models;

namespace TicketingService.Services
{
    public interface ITicketsService
    {
        Task<Ticket> BuyTicket(Ticket ticket);
        Task<Ticket> BuyTicket(int userId, int eventId, DateTime dateTimeBought, int count = 1);
        Task<EventTicketsInfo> GetEventTicketsInfo(int eventId);
        Task<EventTicketsInfo> CreateEventTickets(EventTicketsInfo info);
    }

    public class TicketsService : ITicketsService
    {
        private readonly TicketingDbContext _context;

        public TicketsService(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> BuyTicket(Ticket ticket)
        {
            if (await _context.Tickets.AnyAsync(x => x.DateTimeBought == ticket.DateTimeBought &&
                x.EventId == ticket.EventId && x.UserId == ticket.UserId))
            {
                return null;
            }

            return await BuyTicket(ticket.UserId, ticket.EventId, ticket.DateTimeBought, ticket.Count);
        }

        public async Task<Ticket> BuyTicket(int userId, int eventId, DateTime dateTimeBought, int count = 1)
        {
            try
            {
                var ticket = new Ticket { UserId = userId, EventId = eventId, DateTimeBought = dateTimeBought, Count = count };

                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();

                return ticket;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public async Task<EventTicketsInfo> GetEventTicketsInfo(int eventId)
        {
            var info = await _context.TicketsInfo.SingleOrDefaultAsync(x => x.EventId == eventId);

            return info;
        }

        public async Task<EventTicketsInfo> CreateEventTickets(EventTicketsInfo info)
        {
            await _context.AddAsync(info);
            await _context.SaveChangesAsync();

            return info;
        }
    }
}
