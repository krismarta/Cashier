using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CashierClient.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<Claim> claim = identity.Claims;
                ViewData["is_Authentication"] = "1";
                ViewData["idLogin"] = HttpContext.Session.GetString("idLogin");
                ViewData["Email"] = claim.Where(x => x.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
                ViewData["role"] = claim.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
                if (ViewData["role"].Equals("Cashier"))
                {
                    return RedirectToAction("index", "Transaction");
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("index", "login");
                //ViewData["idLogin"] = "H0123231";
                //ViewData["role"] = "HeadStore";
                //ViewData["Email"] = "kristianto.kt@gmail.com";
            }
            return View();

        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "login");
        }
    }
}
