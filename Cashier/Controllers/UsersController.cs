using Cashier.Base;
using Cashier.Context;
using Cashier.Model;
using Cashier.Repository.Data;
using Cashier.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "HeadStore")]
    public class UsersController : BaseController<User, UserRepository, string>
    {
        private UserRepository userRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public UsersController(MyContext myContext, UserRepository userRepository, IConfiguration configuration) : base(userRepository)
        {
            this.userRepository = userRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpPost("newAccount")]
        public ActionResult<NewAccountVM> newAccount(NewAccountVM newAccountVM)
        {
            var result = userRepository.NewCashier(newAccountVM);
            switch (result)
            {
                case 1:
                    return Ok(result);
                case 0:
                    return NotFound();
                case 2:
                    return Conflict();
                default:
                    return BadRequest();
  
            }
        }

        [HttpPost("changepassword")]
        public ActionResult<User> changepassword(ChangePasswordVM user)
        {
            var result = userRepository.changepassword(user);
            switch (result)
            {
                case 1:
                    return Ok(result);
                case 0:
                    return NotFound();
                default:    
                    return BadRequest();

            }
        }

        [HttpGet("Allcashier")]
        public ActionResult Allcashier()
        {
            var result = userRepository.getAllCashier();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpGet("Counter")]
        public ActionResult Counter()
        {
            var result = userRepository.CounterDashboard();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpGet("Graph1")]
        public ActionResult Graph1()
        {
            var getPayment_midtrans = context.Transactions.Count(r => r.payment_type == "midtrans");
            var getPayment_cash = context.Transactions.Count(r => r.payment_type == "cash");
            var strMid = "Midtrans";
            var strCash = "Cash";

            var series = new List<int>();
            var label = new List<string>();
            series.Add(getPayment_midtrans);
            series.Add(getPayment_cash);
            label.Add(strMid);
            label.Add(strCash);
            var result = new { series, label };

            return Ok(result);
        }

        [HttpGet("Graph2")]
        public ActionResult Graph2()
        {
            var success = context.RequestGoods.Count(r => r.status == "success");
            var pending = context.RequestGoods.Count(r => r.status == "pending");
            var cancel = context.RequestGoods.Count(r => r.status == "cancel");
            var str1 = "Success";
            var str2 = "Pending";
            var str3 = "Cancel";

            var series = new List<int>();
            var label = new List<string>();
            series.Add(success);
            series.Add(pending);
            series.Add(cancel);
            label.Add(str1);
            label.Add(str2);
            label.Add(str3);
            var result = new { series, label };

            return Ok(result);
        }

        [HttpGet("Graph3")]
        public ActionResult Graph3()
        {
            var barang = context.Goods.ToList();
            

            var series = new List<int>();
            var label = new List<string>();
            for (int i = 0; i < barang.Count; i++)
            {
                series.Add(barang[i].stok);
                label.Add(barang[i].name);
            }
            
            
            var result = new { series, label };

            return Ok(result);
        }



    }
}
