using AutoMapper;
using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Events;
using EventService.Services;
using MassTransit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventService.Consumers
{
    public class GetEventConsumer : IConsumer<GetEvent>, IConsumer<GetEventsByArtist>
    {
        private readonly IEventService _service;
        private readonly IMapper _mapper;

        public GetEventConsumer(IEventService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetEvent> context)
        {
            var message = context.Message;

            var @event = await _service.GetEvent(message.Id);

            await context.RespondAsync(_mapper.Map<Models.Event, EventEvent>(@event));
        }

        public async Task Consume(ConsumeContext<GetEventsByArtist> context)
        {
            var message = context.Message;

            var @events = await _service.GetEventsByArtist(message.ArtistId);

            await context.RespondAsync(_mapper.Map<IEnumerable<Models.Event>, IEnumerable<EventEvent>>(@events));
        }
    }
}
