using AutoMapper;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using MassTransit;
using System;
using System.Threading.Tasks;
using TicketingService.Models;
using TicketingService.Services;

namespace TicketingService.Consumers
{
    public class BuyTicketsConsumer : IConsumer<BuyTickets>
    {
        private readonly ITicketsService _service;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public BuyTicketsConsumer(ITicketsService service, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<BuyTickets> context)
        {
            var message = context.Message;

            var ticket = await _service.BuyTicket(message.UserId, message.EventId, DateTime.Now, message.Count);

            var ticketEvent = _mapper.Map<Ticket, TicketBought>(ticket);
            await context.RespondAsync(ticketEvent);
            _eventStoreService.AddEventToStream(ticketEvent, "ticketing-stream");
        }
    }
}
