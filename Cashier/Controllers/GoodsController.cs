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
    public class GoodsController : BaseController<Goods, GoodsRepository, string>
    {
        private GoodsRepository goodsRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public GoodsController(MyContext myContext, GoodsRepository goodsRepository, IConfiguration configuration) : base(goodsRepository)
        {
            this.goodsRepository = goodsRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpGet("AllGoods")]
        public ActionResult AllGoods()
        {
            var result = goodsRepository.getAllGoods();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpGet("getbysupplier/{id_supplier}")]
        public ActionResult<Goods> getGoodsbySupplier(string id_supplier)
        {

            var result = goodsRepository.getGoodsBySupplier(id_supplier);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }






    }
}
