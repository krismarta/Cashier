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
    public class GoodsController : BaseController<Goods, GoodsRepository, string>
    {
        private readonly GoodsRepository goodsRepository;
        public GoodsController(GoodsRepository repository) : base(repository)
        {
            this.goodsRepository = repository;
        }
        [Authorize(Roles = "HeadStore")]
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
        [HttpGet]
        public async Task<JsonResult> getallGoods()
        {
            var result = await goodsRepository.getAllGoods();
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getgoodBySupplier(string id)
        {
            var result = await goodsRepository.GetGoodsbySupplier(id);
            return Json(result);
        }




    }
}
