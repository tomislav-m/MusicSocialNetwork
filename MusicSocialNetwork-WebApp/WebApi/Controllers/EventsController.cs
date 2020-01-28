using Common.MessageContracts.Event.Commands;
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

        public EventsController()
        {

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
    }
}
