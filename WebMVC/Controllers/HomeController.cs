using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using Newtonsoft.Json;
using System.Web;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult SetLanguageEN(string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult SetLanguageUK(string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("uk")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {

            var cookie = HttpContext.Request.Cookies["jwt"];
            if(cookie==null) return RedirectToAction("LogOut");
            var response = CallApi.Get( cookie, "Account/verify");
            if (cookie != null && response.IsSuccessStatusCode) {

                var content = response.Content.ReadAsAsync<Card>();
                CallApi.Role = content.Result.IsAdmin ? CallApi.Roles.admin : CallApi.Roles.user;
                CallApi.Email = content.Result.Email;
                CallApi.CardNumber = content.Result.CardId;
                CallApi.Balance = content.Result.Balance;
                CallApi.Name = content.Result.CardOwnerName;
                return View();
            }
           
            return RedirectToAction("LogOut");
        }

        public IActionResult LogOut()
        {
            CallApi.Role = CallApi.Roles.guest;
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Card emp)
        {

            var result = await CallApi.PostAsync("","account/token",  emp );
   
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Token>();
                readTask.Wait();
                Response.Cookies.Delete("jwt");
                Response.Cookies.Append("jwt", readTask.Result.Access_token);
                return RedirectToAction("Index");
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
