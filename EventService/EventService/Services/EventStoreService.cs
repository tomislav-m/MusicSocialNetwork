using AutoMapper;
using Common.MessageContracts.Event;
using Common.MessageContracts.Event.Event;
using Common.MessageContracts.Event.Events;
using Common.Services;
using EventService.Models;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

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
            Init().Wait();
        }

        public async override Task RecreateDbAsync()
        {
            var events = await ReadFromStreamForward("event-stream");
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
                    case EventMessageContracts.EventEdited:
                        var eventEdited = JsonConvert.DeserializeObject<EventEdited>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _service.EditEvent(_mapper.Map<EventEdited, Event>(eventEdited));
                        break;
                    case EventMessageContracts.EventMarked:
                        var eventMarked = JsonConvert.DeserializeObject<EventMarked>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _service.MarkUserEvent(_mapper.Map<EventMarked, UserEvent>(eventMarked));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
