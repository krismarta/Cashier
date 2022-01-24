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
    public class UsersController : BaseController<User, UserRepository, string>
    {
        private readonly UserRepository userRepository;
        public UsersController(UserRepository repository) : base(repository)
        {
            this.userRepository = repository;
        }

        [HttpPost]
        public JsonResult RegisterAccount(NewAccountVM entity)
        {
            var result = userRepository.RegisterPost(entity);
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult changePassword(ChangePasswordVM entity)
        {
            var result = userRepository.changePassword(entity);
            return Json(result);
        }
        [Authorize]
        public IActionResult MyProfile()
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
        [HttpGet]
        public async Task<JsonResult> getCounter()
        {
            var result = await userRepository.counterdashboardAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GraphUser()
        {
            var result = await userRepository.GraphUser();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GraphUser2()
        {
            var result = await userRepository.GraphUser2();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GraphUser3()
        {
            var result = await userRepository.GraphUser3();
            return Json(result);
        }


    }
}
