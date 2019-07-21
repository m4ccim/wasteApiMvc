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
    public class ProductWastesController : ControllerBase
    {
        private readonly ModelsContext _context;

        public ProductWastesController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/ProductWastes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWaste>>> GetProductWastes()
        {

            return await _context.ProductWastes.ToListAsync();
        }

        // GET: api/ProductWastes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWaste>> GetProductWaste(int id)
        {
            var productWaste = await _context.ProductWastes.FindAsync(id);

            if (productWaste == null)
            {
                return NotFound();
            }

            return productWaste;
        }

        [HttpGet("product/{id}")]
        public async Task<ActionResult<List<ProductWaste>>> GetProductWasteByProduct(int id)
        {
            var productWaste = _context.ProductWastes.Where(x => x.ProductId == id).ToList();


            return productWaste;
        }

        // PUT: api/ProductWastes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductWaste(int id, ProductWaste productWaste)
        {
            if (id != productWaste.Id)
            {
                return BadRequest();
            }

            _context.Entry(productWaste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductWasteExists(id))
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

        // POST: api/ProductWastes
        [HttpPost]
        public async Task<ActionResult<ProductWaste>> PostProductWaste(ProductWaste productWaste)
        {
            _context.ProductWastes.Add(productWaste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductWaste", new { id = productWaste.Id }, productWaste);
        }

        // DELETE: api/ProductWastes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductWaste>> DeleteProductWaste(int id)
        {
            var productWaste = await _context.ProductWastes.FindAsync(id);
            if (productWaste == null)
            {
                return NotFound();
            }

            _context.ProductWastes.Remove(productWaste);
            await _context.SaveChangesAsync();

            return productWaste;
        }

        private bool ProductWasteExists(int id)
        {
            return _context.ProductWastes.Any(e => e.Id == id);
        }
    }
}
