using AutoMapper;
using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Events;
using EventService.Models;
using EventService.Services;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace EventService.Consumers
{
    public class AddEventConsumer : IConsumer<AddEvent>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public AddEventConsumer(IEventService eventService, IMapper mapper, EventStoreService eventStoreService)
        {
            _eventService = eventService;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<AddEvent> context)
        {
            var message = context.Message;

            try
            {
                var eventModel = await _eventService.AddEvent(_mapper.Map<AddEvent, Event>(message));
                var @event = _mapper.Map<Event, EventAdded>(eventModel);
                await context.RespondAsync(@event);
                _eventStoreService.AddEventToStream(@event, "event-stream");
            }
            catch (Exception exc)
            {
                await context.RespondAsync(null);
            }
        }
    }
}
