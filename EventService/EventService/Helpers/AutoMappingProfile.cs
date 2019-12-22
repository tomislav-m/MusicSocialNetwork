using AutoMapper;
using EventService.MessageContracts;
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
