using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            var response = CallApi.Get(Request.Cookies["jwt"], "Products");
            var readTask = response.Content.ReadAsAsync<IList<Product>>();
            readTask.Wait();

            return View(readTask.Result);
        }

        // GET: Products/Details/5

        // GET: Products/Create
        public ActionResult Create()
        {
            var token = Request.Cookies["jwt"];
            return View(new Product());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product collection)
        {
            try
            {
                var token = Request.Cookies["jwt"];
                await CallApi.PostAsync(token, "products", collection);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            var token = Request.Cookies["jwt"];
            ProductProductWastes productProductWastes = new ProductProductWastes();
            productProductWastes.Product = CallApi.Get(token, "products", id.ToString()).Content.ReadAsAsync<Product>().Result;
            var response = CallApi.Get(token, "ProductWastes/product", id.ToString()).Content;
            if (response!=null)
            {
                productProductWastes.productWastes = CallApi.Get(token, "ProductWastes/product", id.ToString()).Content.ReadAsAsync<List<ProductWaste>>().Result;
            }
            return View(productProductWastes);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product collection)
        {
            try
            {
                var token = Request.Cookies["jwt"];
                CallApi.Put(token, "products", collection, id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            var token = Request.Cookies["jwt"];
            return View(CallApi.Get(token, "products", id.ToString()).Content.ReadAsAsync<Product>().Result);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var token = Request.Cookies["jwt"];
                await CallApi.DeleteAsync(token, "products", id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}