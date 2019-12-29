using AutoMapper;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using MassTransit;
using System.Threading.Tasks;
using TicketingService.Models;
using TicketingService.Services;

namespace TicketingService.Consumers
{
    public class GetEventTicketsConsumer : IConsumer<GetEventTickets>
    {
        private readonly ITicketsService _service;
        private readonly IMapper _mapper;

        public GetEventTicketsConsumer(ITicketsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetEventTickets> context)
        {
            var message = context.Message;

            var info = await _service.GetEventTicketsInfo(message.EventId);

            await context.RespondAsync(_mapper.Map<EventTicketsInfo, EventTickets>(info));
        }
    }
}
