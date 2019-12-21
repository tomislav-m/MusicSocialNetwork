using EventService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventService.Services
{
    public interface IEventService
    {
        Task<Event> AddEvent(Event @event);
        Task<Event> GetEvent(int id);
        Task<IEnumerable<Event>> GetEventsByArtist(int artistId);
    }

    public class EventService : IEventService
    {
        private readonly EventsDbContext _context;

        public EventService(EventsDbContext context)
        {
            _context = context;
        }

        public async Task<Event> AddEvent(Event @event)
        {
            await _context.AddAsync(@event);
            await _context.SaveChangesAsync();

            return @event;
        }

        public async Task<IEnumerable<Event>> GetEventsByArtist(int artistId)
        {
            return await _context.Events
                .Where(x => x.EventParticipants.Select(y => y.ArtistId).Contains(artistId))
                .ToListAsync();
        }

        public async Task<Event> GetEvent(int id)
        {
            return await _context.Events.FindAsync(id);
        }
    }
}
