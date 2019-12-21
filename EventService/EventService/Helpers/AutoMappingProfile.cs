using AutoMapper;
using EventService.Commands;
using EventService.Events;
using EventService.Models;

namespace EventService.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AddEvent, Event>();
            CreateMap<Event, EventAdded>();
        }
    }
}
