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
    public class BillProductWastesController : ControllerBase
    {
        private readonly ModelsContext _context;

        public BillProductWastesController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/BillProductWastes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillProductWaste>>> GetBillProductWastes()
        {
            return await _context.BillProductWastes.ToListAsync();
        }

        // GET: api/BillProductWastes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillProductWaste>> GetBillProductWaste(int id)
        {
            var billProductWaste = await _context.BillProductWastes.FindAsync(id);

            if (billProductWaste == null)
            {
                return NotFound();
            }

            return billProductWaste;
        }

        // PUT: api/BillProductWastes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillProductWaste(int id, BillProductWaste billProductWaste)
        {
            if (id != billProductWaste.Id)
            {
                return BadRequest();
            }

            _context.Entry(billProductWaste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillProductWasteExists(id))
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

        // POST: api/BillProductWastes
        [HttpPost]
        public async Task<ActionResult<BillProductWaste>> PostBillProductWaste(BillProductWaste billProductWaste)
        {
            _context.BillProductWastes.Add(billProductWaste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillProductWaste", new { id = billProductWaste.Id }, billProductWaste);
        }

        // DELETE: api/BillProductWastes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillProductWaste>> DeleteBillProductWaste(int id)
        {
            var billProductWaste = await _context.BillProductWastes.FindAsync(id);
            if (billProductWaste == null)
            {
                return NotFound();
            }

            _context.BillProductWastes.Remove(billProductWaste);
            await _context.SaveChangesAsync();

            return billProductWaste;
        }

        private bool BillProductWasteExists(int id)
        {
            return _context.BillProductWastes.Any(e => e.Id == id);
        }
    }
}
