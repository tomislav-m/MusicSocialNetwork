using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Event;
using Common.MessageContracts.Event.Events;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IRequestClient<GetEventsByArtist, EventEvent[]> _requestClient;
        private readonly IRequestClient<GetEvent, EventEvent> _eventRequestClient;
        private readonly IRequestClient<AddEvent, EventAdded> _addEventRequestClient;
        private readonly IRequestClient<EditEvent, EventEdited> _editEventRequestClient;
        private readonly IRequestClient<GetMarkedEvents, MarkedEvent[]> _getMarkedEventsRequestClient;
        private readonly IRequestClient<MarkEvent, EventMarked> _markEventRequestClient;
        private readonly IRequestClient<BuyTickets, TicketBought> _buyTicketsRequestClient;
        private readonly IRequestClient<GetEventTickets, EventTickets> _getTicketsRequestClient;
        private readonly IRequestClient<AddEditEventTickets, EventTicketAdded> _addTicketsRequestClient;

        public EventsController(
            IRequestClient<GetEventsByArtist, EventEvent[]> requestClient,
            IRequestClient<GetEvent, EventEvent> eventRequestClient,
            IRequestClient<AddEvent, EventAdded> addEventRequestClient,
            IRequestClient<EditEvent, EventEdited> editEventRequestClient,
            IRequestClient<GetMarkedEvents, MarkedEvent[]> getMarkedEventsRequestClient,
            IRequestClient<MarkEvent, EventMarked> markEventRequestClient,
            IRequestClient<BuyTickets, TicketBought> buyTicketsRequestClient,
            IRequestClient<GetEventTickets, EventTickets> getTicketsRequestClient,
            IRequestClient<AddEditEventTickets, EventTicketAdded> addTicketsRequestClient)
        {
            _requestClient = requestClient;
            _eventRequestClient = eventRequestClient;
            _addEventRequestClient = addEventRequestClient;
            _editEventRequestClient = editEventRequestClient;
            _getMarkedEventsRequestClient = getMarkedEventsRequestClient;
            _markEventRequestClient = markEventRequestClient;
            _buyTicketsRequestClient = buyTicketsRequestClient;
            _getTicketsRequestClient = getTicketsRequestClient;
            _addTicketsRequestClient = addTicketsRequestClient;
        }

        [HttpGet("artist/{artistId}")]
        public async Task<ActionResult<EventEvent[]>> GetEventsByArtist(int artistId)
        {
            var result = await _requestClient.Request(new GetEventsByArtist { ArtistId = artistId });

            try
            {
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventAdded>> CreateEvent(AddEvent @event)
        {
            var result = await _addEventRequestClient.Request(@event);

            try
            {
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPut]
        public async Task<ActionResult<EventEdited>> EditEvent(EditEvent @event)
        {
            var result = await _editEventRequestClient.Request(@event);

            try
            {
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpGet("marked-events/{userId}")]
        public async Task<ActionResult<MarkedEvent[]>> GetMarkedEvents(int userId)
        {
            var result = await _getMarkedEventsRequestClient.Request(new GetMarkedEvents { UserId = userId });

            try
            {
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost("mark")]
        public async Task<ActionResult<EventMarked>> MarkEvent(MarkEvent markEvent)
        {
            try
            {
                var result = await _markEventRequestClient.Request(markEvent);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost("buy")]
        public async Task<ActionResult<TicketBought>> BuyTickets(BuyTickets buyTickets)
        {
            try
            {
                var result = await _buyTicketsRequestClient.Request(buyTickets);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventTickets>> GetEventTickets(GetEventTickets getEventTickets)
        {
            try
            {
                var result = await _getTicketsRequestClient.Request(getEventTickets);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddEventTicket(AddEditEventTickets ticket)
        {
            try
            {
                var result = await _addTicketsRequestClient.Request(ticket);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost("more")]
        public async Task<ActionResult> GetEvents(int[] ids)
        {
            try
            {
                var events = new List<EventEvent>();

                foreach (var id in ids)
                {
                    var @event = await _eventRequestClient.Request(new GetEvent { Id = id });
                }

                return Ok(events);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
