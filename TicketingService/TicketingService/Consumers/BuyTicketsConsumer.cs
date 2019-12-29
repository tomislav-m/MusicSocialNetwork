using AutoMapper;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using MassTransit;
using System.Threading.Tasks;
using TicketingService.Models;
using TicketingService.Services;

namespace TicketingService.Consumers
{
    public class BuyTicketsConsumer : IConsumer<BuyTickets>
    {
        private readonly ITicketsService _service;
        private readonly IMapper _mapper;

        public BuyTicketsConsumer(ITicketsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<BuyTickets> context)
        {
            var message = context.Message;

            var ticket = await _service.BuyTicket(message.UserId, message.EventId, message.Count);

            await context.RespondAsync(_mapper.Map<Ticket, TicketBought>(ticket));
        }
    }
}
