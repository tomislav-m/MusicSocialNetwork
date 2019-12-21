using AutoMapper;
using EventService.Commands;
using EventService.Events;
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

        public AddEventConsumer(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AddEvent> context)
        {
            var message = context.Message;

            try
            {
                var eventModel = await _eventService.AddEvent(_mapper.Map<AddEvent, Event>(message));
                var @event = _mapper.Map<Event, EventAdded>(eventModel);
                await context.RespondAsync(@event);
                //add to event store
            }
            catch (Exception exc)
            {
                await context.RespondAsync(null);
            }
        }
    }
}
