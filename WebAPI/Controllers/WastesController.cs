using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WastesController : ControllerBase
    {
        private readonly ModelsContext _context;

        public WastesController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/Wastes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Waste>>> GetWastes()
        {
            return await _context.Wastes.ToListAsync();
        }

        // GET: api/Wastes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Waste>> GetWaste(int id)
        {
            var waste = await _context.Wastes.FindAsync(id);

            if (waste == null)
            {
                return NotFound();
            }

            return waste;
        }

        // PUT: api/Wastes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWaste(int id, Waste waste)
        {
            if (id != waste.WasteId)
            {
                return BadRequest();
            }

            var local = _context.Wastes.FirstOrDefault(entry => entry.WasteId.Equals(waste.WasteId));
            _context.Entry(local).State = EntityState.Detached;
            _context.Entry(waste).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WasteExists(id))
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

        // POST: api/Wastes
        [HttpPost]
        public async Task<ActionResult<Waste>> PostWaste(Waste waste)
        {
            _context.Wastes.Add(waste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWaste", new { id = waste.WasteId }, waste);
        }

        // DELETE: api/Wastes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Waste>> DeleteWaste(int id)
        {
            var waste = await _context.Wastes.FindAsync(id);
            if (waste == null)
            {
                return NotFound();
            }

            _context.Wastes.Remove(waste);
            await _context.SaveChangesAsync();

            return waste;
        }

        private bool WasteExists(int id)
        {
            return _context.Wastes.Any(e => e.WasteId == id);
        }
    }
}
