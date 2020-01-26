using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ArtistEvent = Common.MessageContracts.Music.Events.Artist;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IRequestClient<SearchArtist, ArtistFound[]> _requestClient;
        private readonly IRequestClient<GetArtist, ArtistEvent> _getRequestClient;

        public ArtistsController(
            IRequestClient<SearchArtist, ArtistFound[]> requestClient,
            IRequestClient<GetArtist, ArtistEvent> getRequestClient)
        {
            _requestClient = requestClient;
            _getRequestClient = getRequestClient;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return null;
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistEvent>> GetArtist(long id)
        {
            try
            {
                var result = await _getRequestClient.Request(new GetArtist { Id = id });

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

        // PUT: api/Artists/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(long id, Artist artist)
        {
            return NoContent();
        }

        // POST: api/Artists
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Artist>> DeleteArtist(long id)
        {
            return null;
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<ArtistFound[]>> SearchArtist(string searchTerm, int page = 1, int size = 5)
        {
            try
            {
                var result = await _requestClient.Request(new SearchArtist { SearchTerm = searchTerm, Page = page, Size = size });

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
