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
    public class WastesController : Controller
    {
        // GET: Wastes
        public ActionResult Index()
        {
            var response = CallApi.Get(Request.Cookies["jwt"], "Wastes");
            IEnumerable<Waste> result = response.Content.ReadAsAsync<IEnumerable<Waste>>().Result;
            return View(result);
        }

        // GET: Wastes/Details/

        // GET: Wastes/Create
        public ActionResult Create()
        {
            return View(new Waste());
        }

        // POST: Wastes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Waste collection)
        {
            try
            {
                await CallApi.PostAsync(Request.Cookies["jwt"], "Wastes", collection);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wastes/Edit/5
        public ActionResult Edit(int id)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];

            Waste waste = CallApi.Get(cookie, "Wastes", id.ToString()).Content.ReadAsAsync<Waste>().Result;
            return View(waste);
        }

        // POST: Wastes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Waste collection)
        {
            try
            {
                var cookie = HttpContext.Request.Cookies["jwt"];
                var response = CallApi.Put(cookie, "Wastes", collection, id.ToString());

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wastes/Delete/5
        public ActionResult Delete(int id)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];

            Waste waste = CallApi.Get(cookie, "Wastes", id.ToString()).Content.ReadAsAsync<Waste>().Result;
            return View(waste);
        }

        // POST: Wastes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await CallApi.DeleteAsync(Request.Cookies["jwt"], "Wastes", id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}