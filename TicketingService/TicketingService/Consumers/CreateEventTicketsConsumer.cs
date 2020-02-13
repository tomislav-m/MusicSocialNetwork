using AutoMapper;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using MassTransit;
using System.Threading.Tasks;
using TicketingService.Models;
using TicketingService.Services;

namespace TicketingService.Consumers
{
    public class CreateEventTicketsConsumer : IConsumer<AddEditEventTickets>
    {
        private readonly ITicketsService _service;
        private readonly IMapper _mapper;

        public CreateEventTicketsConsumer(ITicketsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AddEditEventTickets> context)
        {
            var message = context.Message;

            var info = await _service.CreateEventTickets(new EventTicketsInfo { Currency = message.Currency, EventId = message.EventId, Price = message.Price, TicketsOverall = message.TicketsOverall });

            await context.RespondAsync(_mapper.Map<EventTicketsInfo, EventTicketAdded>(info));
        }
    }
}
