using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class BillsController : Controller
    {
        // GET: Invoices
        public ActionResult Index()
        {
            return View(CallApi.Get(Request.Cookies["jwt"], "bills").Content.ReadAsAsync<IEnumerable<Bill>>().Result);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int id)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            BillDetails res = CallApi.Get(Request.Cookies["jwt"], "bills/details", id.ToString()).Content.ReadAsAsync<BillDetails>().Result;

            Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();
            foreach (ProductDetails productDetails in res.ProductDetails)
            {
                foreach(WasteDiscount productWaste in productDetails.WasteDiscounts)
                {
                    if (!keyValuePairs.ContainsKey(productWaste.Waste.Name))
                        keyValuePairs.Add(productWaste.Waste.Name, productWaste.DiscountedAmount * productWaste.Waste.RecyclingPrice);
                    else
                    {
                        keyValuePairs[productWaste.Waste.Name] += productWaste.DiscountedAmount * productWaste.Waste.RecyclingPrice;
                    }
                }
            }
            foreach(var x in keyValuePairs)
            {
                dataPoints.Add(new DataPoint(x.Key, x.Value));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(res);

        }

        // GET: Invoices/Create
        [HttpGet]
        public ActionResult Create()
        {
            var date = DateTime.Now;
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);

            Bill bill = CallApi.PostAsync(Request.Cookies["jwt"], "bills", new Bill { DateTime = date}).Result.Content.ReadAsAsync<Bill>().Result;

            return View("Details",new BillDetails { Bill = bill});
        }

        [HttpGet]
        public ActionResult CreateBillProduct(int billId)
        {

            BillProductProducts billProductProducts = new BillProductProducts();
            billProductProducts.products = CallApi.Get(Request.Cookies["jwt"], "products").Content.ReadAsAsync<List<Product>>().Result;

            billProductProducts.billProduct = new BillProduct()
            {
                BillId = billId,
                Product = new Product()
            };

            return View(billProductProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBillProduct(int billId, BillProductProducts collection)
        {

            await CallApi.PostAsync(Request.Cookies["jwt"], "billproducts", collection.billProduct);

            return RedirectToAction("Details", "Bills", new { id = collection.billProduct.BillId });
        }

        //// POST: Invoices/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(InvoiceAndThirdparties collection)
        //{
        //    try
        //    {
        //        Invoice inv = collection.Invoice;
        //        inv.Employee = new Employee();
        //        inv.Employee.Username = CallApi.Email;
        //        await CallApi.PostAsync(Request.Cookies["jwt"], "Invoices", collection.Invoice);

        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Invoices/Delete/5
        public ActionResult Delete(int id)
        {
            return View(CallApi.Get(Request.Cookies["jwt"], "bills", id.ToString()).Content.ReadAsAsync<Bill>().Result);
        }

        // POST: Invoices/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await CallApi.DeleteAsync(Request.Cookies["jwt"], "bills", id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}