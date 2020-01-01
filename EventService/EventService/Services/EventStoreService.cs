using AutoMapper;
using Common.MessageContracts.Event;
using Common.MessageContracts.Event.Events;
using Common.Services;
using EventService.Models;
using Newtonsoft.Json;
using System.Text;

namespace EventService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IEventService _service;

        public EventStoreService(IMapper mapper, IEventService service)
        {
            _mapper = mapper;
            _service = service;
            Init();
        }

        public async override void RecreateDb()
        {
            var events = await ReadFromStream("event-stream");
            foreach(var @event in events)
            {
                var type = @event.Event.EventType;

                switch (type)
                {
                    case EventMessageContracts.EventAdded:
                        var eventAdded = JsonConvert.DeserializeObject<EventAdded>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _service.AddEvent(_mapper.Map<EventAdded, Event>(eventAdded));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
