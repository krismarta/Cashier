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
    public class RequestController : BaseController<RequestGoods, RequestRepository, string>
    {
        private RequestRepository requestRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public RequestController(MyContext myContext, RequestRepository requestRepository, IConfiguration configuration) : base(requestRepository)
        {
            this.requestRepository = requestRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpGet("AllRequest")]
        public ActionResult AllRequest()
        {
            var result = requestRepository.getAllRequest();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpPost("RequestGoods")]
        public ActionResult<RequestGoodsVM> RequestGoods(RequestGoodsVM requestGoodsVM)
        {
            var result = requestRepository.RequestGoods(requestGoodsVM);
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

        [HttpPost("UpdateStatusRequest")]
        public ActionResult UpdateStatusRequest(UpdateStatusVM updateStatusVM)
        {
            var result = requestRepository.UpdateStatusRequest(updateStatusVM);
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





    }
}
