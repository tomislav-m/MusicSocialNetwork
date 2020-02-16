using Common.MessageContracts.Event.Commands;
using EventService.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
        Task<UserEvent> MarkUserEvent(UserEvent userEvent);
        Task<UserEvent[]> GetMarkedEvents(int userId);
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
                    await _context.Headliners.AddAsync(new EventBandHealiner { ArtistId = headliner, EventId = @event.Id });
                }
                foreach (var supporter in @event.Supporters)
                {
                    await _context.Supporters.AddAsync(new EventBandSupporter { ArtistId = supporter, EventId = @event.Id });
                }

                await _context.Events.AddAsync(@event);
                await _context.SaveChangesAsync();
            }

            return @event;
        }

        public async Task<IEnumerable<Event>> GetEventsByArtist(int artistId)
        {
            var headlinerEventBand = _context.Headliners.Where(x => x.ArtistId == artistId);
            var supporterEventBand = _context.Supporters.Where(x => x.ArtistId == artistId);

            try
            {
                var eventIds1 = headlinerEventBand.Where(x => x.ArtistId == artistId)
                    .Select(x => x.EventId);
                var eventIds2 = supporterEventBand.Where(x => x.ArtistId == artistId)
                    .Select(x => x.EventId);

                var events = await _context.Events
                .Where(x => eventIds1.Concat(eventIds2).Distinct().Contains(x.Id))
                .ToListAsync();

                foreach(var @event in events)
                {
                    @event.Headliners = await headlinerEventBand
                        .Where(x => x.EventId == @event.Id).Select(x => x.ArtistId).Distinct()
                        .ToListAsync();
                    @event.Supporters = await supporterEventBand
                        .Where(x => x.EventId == @event.Id).Select(x => x.ArtistId).Distinct()
                        .ToListAsync();
                }

                return events;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public async Task<Event> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {

                var headlinerEventBand = _context.Headliners.Where(x => x.EventId == id).Select(x => x.ArtistId);
                var supporterEventBand = _context.Supporters.Where(x => x.EventId == id).Select(x => x.ArtistId);
                @event.Headliners = await headlinerEventBand.ToListAsync();
                @event.Supporters = await supporterEventBand.ToListAsync();
            }

            return @event;
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
            _context.Supporters.RemoveRange(_context.Supporters.Where(x => x.EventId == oldEvent.Id));

            foreach (var headliner in newEvent.Headliners)
            {
                _context.Headliners.Add(new EventBandHealiner { ArtistId = headliner, EventId = newEvent.Id });
            }
            foreach (var supporter in newEvent.Supporters)
            {
                _context.Supporters.AddAsync(new EventBandSupporter { ArtistId = supporter, EventId = newEvent.Id });
            }
        }

        public async Task<UserEvent> MarkUserEvent(UserEvent userEvent)
        {
            var dbModel = await _context.UserEvents.SingleOrDefaultAsync(x => x.EventId == userEvent.EventId && x.UserId == userEvent.UserId);
            if (dbModel != null)
            {
                if (userEvent.MarkEventType == dbModel.MarkEventType)
                {
                    _context.UserEvents.Remove(dbModel);

                    await _context.SaveChangesAsync();

                    return null;
                }
                else
                {
                    dbModel.MarkEventType = userEvent.MarkEventType;
                    _context.UserEvents.Update(dbModel);

                    await _context.SaveChangesAsync();

                    return dbModel;
                }
            }
            else
            {
                await _context.UserEvents.AddAsync(userEvent);

                await _context.SaveChangesAsync();

                return userEvent;
            }
        }

        public async Task<UserEvent[]> GetMarkedEvents(int userId)
        {
            var events = await _context.UserEvents.Where(x => x.UserId == userId).ToArrayAsync();

            return events;
        }
    }
}
