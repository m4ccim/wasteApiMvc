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
    public class CardWastesController : ControllerBase
    {
        private readonly ModelsContext _context;

        public CardWastesController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/WasteBalances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardWaste>>> GetWasteBalances()
        {
            return await _context.CardWastes.ToListAsync();
        }

        // GET: api/WasteBalances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardWaste>> GetWasteBalance(int id)
        {
            var wasteBalance = await _context.CardWastes.FindAsync(id);

            if (wasteBalance == null)
            {
                return NotFound();
            }

            return wasteBalance;
        }

        [HttpGet("CardId/{id}")]
        public async Task<ActionResult<List<CardWaste>>> GetWasteBalanceByCardId(int id)
        {
            var wasteBalance = _context.CardWastes.Where(x=>x.CardId==id).ToList();

            if (wasteBalance.Count ==0)
            {
                return NotFound();
            }

            return wasteBalance;
        }


        // PUT: api/WasteBalances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWasteBalance(int id, CardWaste wasteBalance)
        {
            if (id != wasteBalance.CardWasteId)
            {
                return BadRequest();
            }
            var local = _context.CardWastes.Find(id);
            _context.Entry(local).State = EntityState.Detached;
            _context.Entry(wasteBalance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WasteBalanceExists(id))
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

        // POST: api/WasteBalances
        [HttpPost]
        public async Task<ActionResult<CardWaste>> PostWasteBalance(CardWaste wasteBalance)
        {
            _context.CardWastes.Add(wasteBalance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWasteBalance", new { id = wasteBalance.CardWasteId }, wasteBalance);
        }

        // DELETE: api/WasteBalances/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CardWaste>> DeleteWasteBalance(int id)
        {
            var wasteBalance = await _context.CardWastes.FindAsync(id);
            if (wasteBalance == null)
            {
                return NotFound();
            }

            _context.CardWastes.Remove(wasteBalance);
            await _context.SaveChangesAsync();

            return wasteBalance;
        }

        private bool WasteBalanceExists(int id)
        {
            return _context.CardWastes.Any(e => e.CardWasteId == id);
        }
    }
}
