using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AlbumEvent = Common.MessageContracts.Music.Events.Album;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IRequestClient<SearchAlbum, AlbumFound[]> _requestClient;
        private readonly IRequestClient<GetAlbum, AlbumEvent> _getRequestClient;
        private readonly IRequestClient<RateAlbum, AlbumRated> _rateRequestClient;
        private readonly IRequestClient<GetRatedAlbums, AlbumRated[]> _catalogRequestClient;
        private readonly IRequestClient<GetAverageRating, AlbumAverageRating> _catalogRatingRequestClient;

        public AlbumsController(
            IRequestClient<SearchAlbum, AlbumFound[]> requestClient,
            IRequestClient<GetAlbum, AlbumEvent> getRequestClient,
            IRequestClient<RateAlbum, AlbumRated> rateRequestClient,
            IRequestClient<GetRatedAlbums, AlbumRated[]> catalogRequestClient,
            IRequestClient<GetAverageRating, AlbumAverageRating> catalogRatingRequestClient)
        {
            _requestClient = requestClient;
            _getRequestClient = getRequestClient;
            _rateRequestClient = rateRequestClient;
            _catalogRequestClient = catalogRequestClient;
            _catalogRatingRequestClient = catalogRatingRequestClient;
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            return null;
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumEvent>> GetAlbum(long id)
        {
            try
            {
                var result = await _getRequestClient.Request(new GetAlbum { Id = id });

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

        // PUT: api/Albums/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(long id, Album album)
        {
            return NoContent();
        }

        // POST: api/Albums
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Album>> DeleteAlbum(long id)
        {
            return null;
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<AlbumFound[]>> SearchAlbum(string searchTerm, int page = 1, int size = 5)
        {
            try
            {
                var result = await _requestClient.Request(new { searchTerm, page, size });

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

        [HttpPost("rate")]
        public async Task<ActionResult<AlbumRated>> RateAlbum([FromBody]RateAlbum rateAlbum)
        {
            try
            {
                var result = await _rateRequestClient.Request(new RateAlbum { UserId = rateAlbum.UserId, AlbumId = rateAlbum.AlbumId, Rating = rateAlbum.Rating });

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

        [HttpGet("rated/{userId}")]
        public async Task<ActionResult<AlbumRated[]>> GetRatedAlbums(int userId)
        {
            try
            {
                var ratedAlbums = await _catalogRequestClient.Request(new GetRatedAlbums { Id = userId });

                if (ratedAlbums == null)
                {
                    return NotFound();
                }

                return Ok(ratedAlbums);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        [HttpGet("average-rating/{albumId}")]
        public async Task<ActionResult<AlbumAverageRating>> GetAverageRating(int albumId)
        {
            try
            {
                var averageRating = await _catalogRatingRequestClient.Request(new GetAverageRating { AlbumId = albumId });

                return Ok(averageRating);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
