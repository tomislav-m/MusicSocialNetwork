using CatalogService.MessageContract;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Model;
using WebApi.Model.InternalModels;
using Album = WebApi.Model.Album;
using AlbumEvent = MusicService.MessageContracts.Album;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly MusicDbContext _context;
        private readonly IRequestClient<SearchAlbum, AlbumFound[]> _requestClient;
        private readonly IRequestClient<GetAlbum, AlbumEvent> _getRequestClient;
        private readonly IRequestClient<RateAlbum, AlbumRated> _rateRequestClient;

        public AlbumsController(MusicDbContext context,
            IRequestClient<SearchAlbum, AlbumFound[]> requestClient,
            IRequestClient<GetAlbum, AlbumEvent> getRequestClient,
            IRequestClient<RateAlbum, AlbumRated> rateRequestClient)
        {
            _context = context;
            _requestClient = requestClient;
            _getRequestClient = getRequestClient;
            _rateRequestClient = rateRequestClient;
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            var albums = await _context.Albums.Include(x => x.Tracks).Where(x => !x.Tracks.Any()).ToListAsync();
            return albums;
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
            if (id != album.Id)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Albums
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            var entity = await _context.Albums.FirstOrDefaultAsync(x => x.MbId == album.MbId && x.Name == album.Name);
            if (entity != null)
            {
                return Conflict();
            }

            if (album.FormatStr != null)
            {
                var format = await _context.Formats.FirstOrDefaultAsync(x => x.Title.ToLower() == album.FormatStr.ToLower());
                if (format == null)
                {
                    format = new Format { Title = album.FormatStr };
                }
                album.Format = format;
            }

            if (album.StyleStr != null)
            {
                var style = await _context.Styles.FirstOrDefaultAsync(x => x.Title.ToLower() == album.StyleStr.ToLower());
                if (style == null)
                {
                    style = new Style { Title = album.StyleStr };
                }
                album.Style = style;
            }

            if (album.GenreStr != null)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Title.ToLower() == album.GenreStr.ToLower());
                if (genre == null)
                {
                    genre = new Genre { Title = album.GenreStr };
                }
                album.Genre = genre;
            }

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Album>> DeleteAlbum(long id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return album;
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

        private bool AlbumExists(long id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
