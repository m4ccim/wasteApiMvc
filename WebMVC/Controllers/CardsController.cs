using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class CardsController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            var response = CallApi.Get(Request.Cookies["jwt"], "Cards");
            IEnumerable<Card> result = response.Content.ReadAsAsync<IEnumerable<Card>>().Result;

            return View(result);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {

            return View(new Card());
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Card collection)
        {
            try
            {
                await CallApi.PostAsync(Request.Cookies["jwt"], "Cards", collection);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Profile()
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            
            CardCardWastes cardCardWastes = new CardCardWastes();
            cardCardWastes.Card = CallApi.Get(cookie, "Cards", CallApi.CardNumber.ToString()).Content.ReadAsAsync<Card>().Result;
            cardCardWastes.CardWastes = CallApi.Get(cookie, "CardWastes/CardId", CallApi.CardNumber.ToString()).Content.ReadAsAsync<List<CardWaste>>().Result;

            return View(cardCardWastes);
        }

        [HttpPost]
        public ActionResult Profile(Card card)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            var response = CallApi.Put(cookie, "Cards", card, card.CardId.ToString());



            return RedirectToAction("Index", "Home");
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            CardCardWastes cardCardWastes = new CardCardWastes();
            cardCardWastes.Card = CallApi.Get(cookie, "Cards", id.ToString()).Content.ReadAsAsync<Card>().Result;
            cardCardWastes.CardWastes = CallApi.Get(cookie, "CardWastes/CardId", id.ToString()).Content.ReadAsAsync<List<CardWaste>>().Result;
            return View(cardCardWastes);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Card card)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            var response = CallApi.Put(cookie, "Cards", card, id.ToString());

            return RedirectToAction(nameof(Index));

        }

        // GET: Employee/Edit/5
        public ActionResult EditCardWaste(int id)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            CardWaste cw = CallApi.Get(cookie, "CardWastes", id.ToString()).Content.ReadAsAsync<CardWaste>().Result;
            return View(cw);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCardWaste(int id, CardWaste cardWaste)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            var response = CallApi.Put(cookie, "CardWastes", cardWaste, id.ToString());

            return RedirectToAction("Edit", new { id = cardWaste.CardId});

        }

        public async Task<ActionResult> ResetBalance(int id)
        {
            var cookie = HttpContext.Request.Cookies["jwt"];
            var response = await CallApi.PostAsync(cookie, "Cards/resetbalance/"+id, new object());
            return RedirectToAction("Edit", new {id});
        }


        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            Card card = CallApi.Get(Request.Cookies["jwt"], "cards", id.ToString()).Content.ReadAsAsync<Card>().Result;
            return View(card);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Card collection)
        {
            try
            {
                await CallApi.DeleteAsync(Request.Cookies["jwt"], "cards", id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}