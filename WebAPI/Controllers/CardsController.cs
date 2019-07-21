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
    public class CardsController : ControllerBase
    {
        private readonly ModelsContext _context;

        public CardsController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards.ToListAsync();
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Card>> Login(Card card)
        {
            var existingCard = await _context.Cards.FirstOrDefaultAsync(e => e.Email == card.Email);
            if (card == null)
            {
                return NotFound();
            }
            bool isVerified = Hashing.VerifyHashedPassword(existingCard.PasswordHash, card.PasswordHash);

            if (isVerified)
                return existingCard;
            else return BadRequest();
        }

        // PUT: api/Cards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(int id, Card card)
        {
            if (id != card.CardId)
            {
                return BadRequest();
            }
            var existingCard = _context.Cards.Find(id);
            if (card.PasswordHash != existingCard.PasswordHash)
            {
                card.PasswordHash = Hashing.HashPassword(card.PasswordHash);
            }
            if(card.Balance==0)
                 card.Balance = existingCard.Balance;
            _context.Entry(existingCard).CurrentValues.SetValues(card);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // POST: api/Cards
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(Card card)
        {
            if(_context.Cards.FirstOrDefault(x => x.Email == card.Email) != null)
            {
                return BadRequest();
            } 
           
            card.PasswordHash = Hashing.HashPassword(card.PasswordHash);
            _context.Cards.Add(card);

            await _context.SaveChangesAsync();
            ResetBalance(card.CardId);
            return CreatedAtAction("GetCard", new { id = card.CardId }, card);
        }

        [HttpPost("resetbalance/{CardId}")]
        public async Task<ActionResult<List<CardWaste>>> ResetBalance(int CardId)
        {
            Card card = _context.Cards.Find(CardId);
            int cardId = card.CardId;
            foreach (Waste waste in _context.Wastes.ToList())
            {
                CardWaste wb = _context.CardWastes.FirstOrDefault(x => x.CardId == cardId && x.WasteId == waste.WasteId);
                if(wb != null)
                {
                    card.Balance += (wb.Amount * wb.Waste.RecyclingPrice) / 5;
                    wb.Amount = waste.StartAmount;
                    _context.Entry(wb).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else if (wb == null)
                {
                    _context.CardWastes.Add(new CardWaste { WasteId = waste.WasteId, CardId = cardId, Amount = waste.StartAmount });
                    await _context.SaveChangesAsync();
                }
            }
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.CardWastes.Where(x => x.CardId == CardId).ToList();
        }

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Card>> DeleteCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return card;
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }
}
