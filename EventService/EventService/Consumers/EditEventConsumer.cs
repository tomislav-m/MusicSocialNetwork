using AutoMapper;
using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Event;
using EventService.Models;
using EventService.Services;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace EventService.Consumers
{
    public class EditEventConsumer : IConsumer<EditEvent>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public EditEventConsumer(IEventService eventService, IMapper mapper, EventStoreService eventStoreService)
        {
            _eventService = eventService;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<EditEvent> context)
        {
            var message = context.Message;

            try
            {
                var eventModel = await _eventService.EditEvent(_mapper.Map<EditEvent, Event>(message));
                var @event = _mapper.Map<Event, EventEdited>(eventModel);
                await context.RespondAsync(@event);
                _eventStoreService.AddEventToStream(@event, "event-stream");
            }
            catch (Exception exc)
            {
                var @event = new EventEdited { Exception = exc };
                await context.RespondAsync(@event);
            }
        }
    }
}
