using Cashier.Model;
using Cashier.ViewModel;
using CashierClient.Base.Controllers;
using CashierClient.Models;
using CashierClient.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CashierClient.Controllers
{
    public class TransactionController : BaseController<Transaction, TransactionRepository, string>
    {
        private readonly TransactionRepository transactionRepository;
        public TransactionController(TransactionRepository repository) : base(repository)
        {
            this.transactionRepository = repository;
        }
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
        public IActionResult Report()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<Claim> claim = identity.Claims;
                ViewData["is_Authentication"] = "1";
                ViewData["idLogin"] = HttpContext.Session.GetString("idLogin");
                ViewData["Email"] = claim.Where(x => x.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
                ViewData["role"] = claim.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            }
            else
            {
                //HttpContext.Session.Clear();
                //return RedirectToAction("index", "login");
                ViewData["idLogin"] = "H0123231";
                ViewData["role"] = "HeadStore";
                ViewData["Email"] = "kristianto.kt@gmail.com";
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getAllTransaction()
        {
            var result = await transactionRepository.getAllTransaction();
            return Json(result);
        }

        [HttpPost]
        public JsonResult TransactionInsert(TransactionVM entity)
        {
            var result = transactionRepository.TransactionInsert(entity);
            return Json(result);
        }
        [HttpPost]
        public JsonResult PaymentWithMidtrans(TransactionVM entity)
        {
            var result = transactionRepository.PaymentWithMidtrans(entity);
            return Json(result);
        }

        [HttpGet]
        public JsonResult StatusPayment(string id)
        {
            var result = transactionRepository.StatusMidtrans(id);
            return Json(result);
        }

    }
}
