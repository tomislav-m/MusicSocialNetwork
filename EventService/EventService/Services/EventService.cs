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
        Task<Event> EditEvent(int id, Event @event);
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
            if ((await GetEvent(@event.Id)) == null)
            {
                await _context.AddAsync(@event);
                await _context.SaveChangesAsync();
            }

            return @event;
        }

        public async Task<IEnumerable<Event>> GetEventsByArtist(int artistId)
        {
            return await _context.Events
                .Where(x => x.Headliners.Concat(x.Supporters).Contains(artistId))
                .ToListAsync();
        }

        public async Task<Event> GetEvent(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event> EditEvent(int id, Event @event)
        {
            var model = await _context.Events.FindAsync(id);

            if (model != null)
            {
                MapEventEntity(model, @event);
                await _context.SaveChangesAsync();

                return @event;
            }

            return null;
        }

        private void MapEventEntity(Event oldEvent, Event newEvent)
        {
            oldEvent.Date = newEvent.Date;
            oldEvent.Headliners = newEvent.Headliners;
            oldEvent.Supporters = newEvent.Supporters;
            oldEvent.Type = newEvent.Type;
            oldEvent.Venue = newEvent.Venue;
        }
    }
}
