using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Event;
using Common.MessageContracts.Event.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IRequestClient<GetEventsByArtist, EventEvent[]> _requestClient;
        private readonly IRequestClient<AddEvent, EventAdded> _addEventRequestClient;
        private readonly IRequestClient<EditEvent, EventEdited> _editEventRequestClient;

        public EventsController(
            IRequestClient<GetEventsByArtist, EventEvent[]> requestClient,
            IRequestClient<AddEvent, EventAdded> addEventRequestClient,
            IRequestClient<EditEvent, EventEdited> editEventRequestClient)
        {
            _requestClient = requestClient;
            _addEventRequestClient = addEventRequestClient;
            _editEventRequestClient = editEventRequestClient;
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
    }
}
