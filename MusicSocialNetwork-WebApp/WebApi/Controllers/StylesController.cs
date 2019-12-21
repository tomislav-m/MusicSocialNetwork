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
    public class StylesController : ControllerBase
    {
        private readonly MusicDbContext _context;

        public StylesController(MusicDbContext context)
        {
            _context = context;
        }

        // GET: api/Styles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Style>>> GetStyles()
        {
            return await _context.Styles.ToListAsync();
        }

        // GET: api/Styles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Style>> GetStyle(long id)
        {
            var style = await _context.Styles.FindAsync(id);

            if (style == null)
            {
                return NotFound();
            }

            return style;
        }

        // PUT: api/Styles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStyle(long id, Style style)
        {
            if (id != style.Id)
            {
                return BadRequest();
            }

            _context.Entry(style).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StyleExists(id))
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

        // POST: api/Styles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Style>> PostStyle(Style style)
        {
            _context.Styles.Add(style);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStyle", new { id = style.Id }, style);
        }

        // DELETE: api/Styles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Style>> DeleteStyle(long id)
        {
            var style = await _context.Styles.FindAsync(id);
            if (style == null)
            {
                return NotFound();
            }

            _context.Styles.Remove(style);
            await _context.SaveChangesAsync();

            return style;
        }

        private bool StyleExists(long id)
        {
            return _context.Styles.Any(e => e.Id == id);
        }
    }
}
