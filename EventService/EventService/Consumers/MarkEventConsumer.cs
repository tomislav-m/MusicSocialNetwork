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
    public class MarkEventConsumer : IConsumer<MarkEvent>, IConsumer<GetMarkedEvents>
    {
        private readonly IEventService _service;
        private readonly EventStoreService _eventStoreService;
        private readonly IMapper _mapper;

        public MarkEventConsumer(IEventService service, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _eventStoreService = eventStoreService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MarkEvent> context)
        {
            var message = context.Message;

            try
            {
                var eventModel = await _service.MarkUserEvent(new UserEvent { EventId = message.EventId, UserId = message.UserId, MarkType = (int)message.MarkEventType });
                var @event = _mapper.Map<UserEvent, EventMarked>(eventModel);
                await context.RespondAsync(@event);
                _eventStoreService.AddEventToStream(@event, "event-stream");
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new EventMarked { Exception = exc });
            }
        }

        public async Task Consume(ConsumeContext<GetMarkedEvents> context)
        {
            var message = context.Message;

            try
            {
                var events = await _service.GetMarkedEvents(message.UserId);
                await context.RespondAsync(_mapper.Map<UserEvent[], MarkedEvent[]>(events));
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new MarkedEvent[]{ new MarkedEvent { Exception = exc } } );
            }
        }
    }
}
