using AutoMapper;
using Common.MessageContracts.Ticketing;
using Common.MessageContracts.Ticketing.Events;
using Common.Services;
using Newtonsoft.Json;
using System.Text;
using TicketingService.Models;

namespace TicketingService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ITicketsService _service;

        public EventStoreService(IMapper mapper, TicketsService service)
        {
            _mapper = mapper;
            _service = service;
            Init();
        }

        public async override void RecreateDb()
        {
            var events = await ReadFromStream("ticketing-stream");

            foreach (var @event in events)
            {
                var type = @event.Event.EventType;

                switch (type)
                {
                    case TicketingMessageContracts.TicketBought:
                        var ticketBoughtEvent = JsonConvert.DeserializeObject<TicketBought>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _service.BuyTicket(_mapper.Map<TicketBought, Ticket>(ticketBoughtEvent));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
