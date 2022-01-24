using Cashier.Model;
using Cashier.ViewModel;
using CashierClient.Base.Controllers;
using CashierClient.Models;
using CashierClient.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CashierClient.Controllers
{
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.accountRepository = repository;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("Auth/")]
        public JsonResult Auth(LoginVM login)
        {
            var result = accountRepository.Auth(login);
            if (result.Result.idtoken != null)
            {
                //return RedirectToAction("index");
                result.Result.statusCode = "1";
                HttpContext.Session.SetString("JWToken", result.Result.idtoken);
                HttpContext.Session.SetString("Email", result.Result.email);
                HttpContext.Session.SetString("idLogin", result.Result.idLogin);
            }
            else
            {
                result.Result.statusCode = "0";
            }


            return Json(result);
        }

        [HttpPost]
        public JsonResult Forgot(LoginVM forgotPasswordVM)
        {
            var result = accountRepository.Forgot(forgotPasswordVM);
            return Json(result);
        }

    }
}
