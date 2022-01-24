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
    public class RequestController : BaseController<RequestGoods, RequestRepository, string>
    {
        private readonly RequestRepository requestRepository;
        public RequestController(RequestRepository repository) : base(repository)
        {
            this.requestRepository = repository;
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
        [Authorize(Roles = "HeadStore")]
        public IActionResult report()
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

        [HttpPost]
        public JsonResult requestGoodsSupplier(RequestGoodsVM entity)
        {
            var result = requestRepository.RequestGoodsSupplier(entity);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getAllRequest()
        {
            var result = await requestRepository.getAllRequest();
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateStatusRequest(UpdateStatusVM entity)
        {
            var result = requestRepository.UpdateStatusRequest(entity);
            return Json(result);
        }




    }
}
