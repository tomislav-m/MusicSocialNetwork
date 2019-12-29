using AutoMapper;
using Common.MessageContracts.Ticketing.Events;
using TicketingService.Models;

namespace TicketingService.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Ticket, TicketBought>();
            CreateMap<EventTicketsInfo, EventTickets>();
        }
    }
}
