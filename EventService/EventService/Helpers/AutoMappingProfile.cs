using AutoMapper;
using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Events;
using EventService.Models;

namespace EventService.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AddEvent, Event>();
            CreateMap<Event, EventAdded>();

            CreateMap<EventAdded, Event>();
        }
    }
}
