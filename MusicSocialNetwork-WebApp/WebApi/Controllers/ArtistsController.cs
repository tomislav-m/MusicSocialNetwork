using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRequestClient<GetArtistNamesByIds, ArtistSimple[]> _getNamesRequestClient;
        private readonly IRequestClient<CreateArtist, ArtistCreated> _createArtistRequestClient;
        private readonly IRequestClient<EditArtist, ArtistEdited> _editArtistRequestClient;

        public ArtistsController(
            IRequestClient<SearchArtist, ArtistFound[]> requestClient,
            IRequestClient<GetArtist, ArtistEvent> getRequestClient,
            IRequestClient<GetArtistNamesByIds, ArtistSimple[]> getNamesRequestClient,
            IRequestClient<CreateArtist, ArtistCreated> createArtistRequestClient,
            IRequestClient<EditArtist, ArtistEdited> editArtistRequestClient)
        {
            _requestClient = requestClient;
            _getRequestClient = getRequestClient;
            _getNamesRequestClient = getNamesRequestClient;
            _createArtistRequestClient = createArtistRequestClient;
            _editArtistRequestClient = editArtistRequestClient;
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
        public async Task<ActionResult<ArtistEdited>> PutArtist(long id, Artist artist)
        {
            try
            {
                var result = await _editArtistRequestClient.Request(artist);
                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(CreateArtist artist)
        {
            try
            {
                var result = await _createArtistRequestClient.Request(artist);

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
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

        [HttpPost("names")]
        public async Task<ActionResult<ArtistSimple[]>> GetArtistNames(int[] artistIds)
        {
            try
            {
                var result = await _getNamesRequestClient.Request(new GetArtistNamesByIds { Ids = artistIds });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result.ToDictionary(x => x.Id, x => x.Name));
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
