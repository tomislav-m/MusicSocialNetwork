using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Model;
using Artist = WebApi.Model.Artist;
using ArtistEvent = Common.MessageContracts.Music.Events.Artist;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly MusicDbContext _context;
        private readonly IRequestClient<SearchArtist, ArtistFound[]> _requestClient;
        private readonly IRequestClient<GetArtist, ArtistEvent> _getRequestClient;

        public ArtistsController(MusicDbContext context,
            IRequestClient<SearchArtist, ArtistFound[]> requestClient,
            IRequestClient<GetArtist, ArtistEvent> getRequestClient)
        {
            _context = context;
            _requestClient = requestClient;
            _getRequestClient = getRequestClient;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            var artists = await _context.Artists.Include(x => x.Albums).ToListAsync();
            var artists2 = new List<Artist>();
            foreach (var artist in artists)
            {
                if (artist.Albums.Any(x => x.TMDBId == null))
                {
                    artists2.Add(artist);
                }
            }

            return artists2;
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
            if (id != artist.Id)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Artists
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            if (await _context.Artists.AnyAsync(x => x.Name == artist.Name && x.MbId == artist.MbId))
            {
                return Conflict();
            }

            _context.Artists.Add(artist);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArtistExists(artist.MbId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Artist>> DeleteArtist(long id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return artist;
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<ArtistFound[]>> SearchArtist(string searchTerm, int page = 1, int size = 5)
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

        private bool ArtistExists(string mbId)
        {
            return _context.Artists.Any(e => e.MbId == mbId);
        }
    }
}
