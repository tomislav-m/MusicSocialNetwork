﻿using EventService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventService.Services
{
    public interface IEventService
    {
        Task<Event> AddEvent(Event @event);
        Task<Event> EditEvent(Event @event);
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
                foreach(var headliner in @event.Headliners)
                {
                    await _context.Headliners.AddAsync(new EventBand { ArtistId = headliner, EventId = @event.Id });
                }
                foreach (var supporter in @event.Supporters)
                {
                    await _context.Supporters.AddAsync(new EventBand { ArtistId = supporter, EventId = @event.Id });
                }

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

        public async Task<Event> EditEvent(Event @event)
        {
            var model = await _context.Events.FindAsync(@event.Id);

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
            oldEvent.Type = newEvent.Type;
            oldEvent.Venue = newEvent.Venue;

            _context.Headliners.RemoveRange(_context.Headliners.Where(x => x.EventId == oldEvent.Id));
            _context.Supporters.RemoveRange(_context.Headliners.Where(x => x.EventId == oldEvent.Id));

            foreach (var headliner in newEvent.Headliners)
            {
                _context.Headliners.Add(new EventBand { ArtistId = headliner, EventId = newEvent.Id });
            }
            foreach (var supporter in newEvent.Supporters)
            {
                _context.Supporters.AddAsync(new EventBand { ArtistId = supporter, EventId = newEvent.Id });
            }
        }
    }
}
