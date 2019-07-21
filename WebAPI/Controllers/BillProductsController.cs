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
    public class BillProductsController : ControllerBase
    {
        private readonly ModelsContext _context;

        public BillProductsController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/BillProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillProduct>>> GetBillProducts()
        {
            return await _context.BillProducts.ToListAsync();
        }

        // GET: api/BillProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillProduct>> GetBillProduct(int id)
        {
            var billProduct = await _context.BillProducts.FindAsync(id);

            if (billProduct == null)
            {
                return NotFound();
            }

            return billProduct;
        }

        // PUT: api/BillProducts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillProduct(int id, BillProduct billProduct)
        {
            if (id != billProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(billProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillProductExists(id))
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

        // POST: api/BillProducts
        [HttpPost]
        public async Task<ActionResult<BillProduct>> PostBillProduct(BillProduct billProduct)
        {
            if (billProduct.ProductId == 0)
            {
                billProduct.ProductId = billProduct.Product.ProductId;

                billProduct.Product = null;
            }
            _context.BillProducts.Add(billProduct);
            await _context.SaveChangesAsync();

            var bill = _context.Bills.Find(billProduct.BillId);
            billProduct.Bill.Total += billProduct.Amount * _context.Products.Find(billProduct.ProductId).BasePrice;
            _context.Entry(bill).State = EntityState.Detached;
            _context.Entry(billProduct.Bill).State = EntityState.Modified;
            _context.SaveChanges();
            return CreatedAtAction("GetBillProduct", new { id = billProduct.BillId }, billProduct);
        }

        // DELETE: api/BillProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillProduct>> DeleteBillProduct(int id)
        {
            var billProduct = await _context.BillProducts.FindAsync(id);
            if (billProduct == null)
            {
                return NotFound();
            }

            _context.BillProducts.Remove(billProduct);
            await _context.SaveChangesAsync();

            return billProduct;
        }

        private bool BillProductExists(int id)
        {
            return _context.BillProducts.Any(e => e.Id == id);
        }
    }
}
