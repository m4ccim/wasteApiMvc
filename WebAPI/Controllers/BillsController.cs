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
    public class BillsController : ControllerBase
    {
        private readonly ModelsContext _context;

        public BillsController(ModelsContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        {
            return await _context.Bills.ToListAsync();
        }

        [HttpGet("unCompleted")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetUncompletedBills()
        {
            List<Bill> bills = _context.Bills.Where(x => x.IsCompleted == false).ToList();
            return bills;
        }


        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }

        [HttpGet("cardId/{id}")]
        public async Task<ActionResult<List<Bill>>> GetBills(int id)
        {
            var bills = _context.Bills.Where(x=>x.CardId==id).ToList();

            if (bills.Count==0)
            {
                return NotFound();
            }

            return bills;
        }


        // PUT: api/Bills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.BillId)
            {
                return BadRequest();
            }

            _context.Entry(bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
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

        public struct BillList
        {
            public Bill bill;
            public List<BillProduct> billProducts;
        }

        [HttpPost()]
        public async Task<ActionResult<Bill>> PreCreateBill(Bill bill)
        {
            bill.CardId = 1;
            bill.BillId = 0;
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBill", new { id = bill.BillId }, bill);
        }

        // POST: api/Bills
        [HttpPost("Create/{id}/{cardId}")]
        public async Task<ActionResult<BillList>> CreateBill(int id, int cardId)
        {
            BillList billList = new BillList()
            {
                bill = _context.Bills.Find(id),
                billProducts = _context.BillProducts.Where(x => x.BillId == id).ToList()

            };
            billList.bill.CardId = cardId;

            List<BillProduct> billProducts = billList.billProducts;
            if (billProducts == null || billProducts.Count == 0)
                return BadRequest();
            using (var transaction = _context.Database.BeginTransaction())
            {

                //foreach (BillProduct billProduct in billProducts)
                //{
                //    billProduct.BillId = billList.bill.BillId;
                //}
                //_context.BillProducts.AddRange(billProducts);
                //await _context.SaveChangesAsync();

                double sum = 0.0;
                double discount = 0.0;
                foreach (BillProduct billProduct in billProducts)
                {
                    sum += billProduct.Amount * _context.Products.Find(billProduct.ProductId).BasePrice;
                    List<CardWaste> wasteBalances = _context.CardWastes.Where(x => x.CardId == billList.bill.CardId).ToList();
                    List<ProductWaste> productWastes = _context.ProductWastes.Where(x => x.ProductId == billProduct.ProductId).ToList();

                    foreach (ProductWaste productWaste in productWastes)
                    {
                        double totalAmount = productWaste.Amount * billProduct.Amount;
                        CardWaste cardWaste = wasteBalances.FirstOrDefault(x => x.WasteId == productWaste.WasteId);
                        totalAmount -= cardWaste.Amount;
                        if (totalAmount >= 0)
                        {
                            cardWaste.Amount -= productWaste.Amount * billProduct.Amount - totalAmount;
                        }
                        else
                        {
                            cardWaste.Amount = -totalAmount;
                            totalAmount = 0;
                        }
                        _context.Entry(cardWaste).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        discount += (productWaste.Amount * billProduct.Amount - totalAmount) * productWaste.Waste.RecyclingPrice;
                        _context.BillProductWastes.Add(new BillProductWaste { BillProductId = billProduct.Id, WasteId = productWaste.WasteId, DiscountedAmount = productWaste.Amount * billProduct.Amount - totalAmount });
                        await _context.SaveChangesAsync();
                    }
                }
                double total = sum - discount;
                if (_context.Cards.Find(billList.bill.CardId).Balance < total)
                {
                    transaction.Rollback();
                    return StatusCode(402);
                }
                billList.bill.Discount = discount;
                billList.bill.Total = total;
                billList.bill.IsCompleted = true;
                _context.Entry(billList.bill).State = EntityState.Modified;
                _context.Cards.Find(billList.bill.CardId).Balance -= total;
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            return Redirect("Details/"+billList.bill.BillId);

        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult<object>> GetBillDetails(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            var result = new BillDetails
            {
                Bill = bill,
                ProductDetails = _context.BillProducts.Where(x => x.BillId == bill.BillId)
                .Select(y => new ProductDetails{
                    Product = y.Product,
                    Amount = y.Amount,
                    WasteDiscounts = _context.BillProductWastes.Where(z => z.BillProductId == y.Id)
                    .Select(z=> new WasteDiscount {Waste =  z.Waste, DiscountedAmount = z.DiscountedAmount }).ToList()
                }).ToList(),
            };


            return result;
        }


        [HttpGet("Stats/{fromDate}/{toDate}")]
        public async Task<ActionResult<object>> GetBillStats(DateTime fromDate, DateTime toDate)
        {
            List<Bill> bills = _context.Bills.Where(x => (DateTime.Compare(x.DateTime, fromDate) >= 0) &&
                (DateTime.Compare(x.DateTime, toDate) <= 0)).ToList();
            var result = new
            {
                Total = bills.Sum(x => x.Total),
                Discount = bills.Sum(x => x.Discount),
                Wastes = new List<object>()
                {
                    new
                    {
                        Name = _context.BillProductWastes.GroupBy(a => a.WasteId)
                                .Select(a => new {Name = a.Select(b => b.Waste.Name).FirstOrDefault(),
                                    DiscountedAmount = a.Sum(b => b.DiscountedAmount),
                                    DiscountedSum = a.Sum(b=>b.Waste.RecyclingPrice*b.DiscountedAmount) })

                    }
                }
            };
            return result;
        }



        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bill>> DeleteBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return bill;
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }
    }
}
