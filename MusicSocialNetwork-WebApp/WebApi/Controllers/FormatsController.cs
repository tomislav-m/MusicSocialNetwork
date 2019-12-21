using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;
using WebApi.Model.InternalModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatsController : ControllerBase
    {
        private readonly MusicDbContext _context;

        public FormatsController(MusicDbContext context)
        {
            _context = context;
        }

        // GET: api/Formats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Format>>> GetFormats()
        {
            return await _context.Formats.ToListAsync();
        }

        // GET: api/Formats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Format>> GetFormat(long id)
        {
            var format = await _context.Formats.FindAsync(id);

            if (format == null)
            {
                return NotFound();
            }

            return format;
        }

        // PUT: api/Formats/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormat(long id, Format format)
        {
            if (id != format.Id)
            {
                return BadRequest();
            }

            _context.Entry(format).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormatExists(id))
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

        // POST: api/Formats
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Format>> PostFormat(Format format)
        {
            _context.Formats.Add(format);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormat", new { id = format.Id }, format);
        }

        // DELETE: api/Formats/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Format>> DeleteFormat(long id)
        {
            var format = await _context.Formats.FindAsync(id);
            if (format == null)
            {
                return NotFound();
            }

            _context.Formats.Remove(format);
            await _context.SaveChangesAsync();

            return format;
        }

        private bool FormatExists(long id)
        {
            return _context.Formats.Any(e => e.Id == id);
        }
    }
}
