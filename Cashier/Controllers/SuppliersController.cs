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
    public class SuppliersController : BaseController<Supplier, SupplierRepository, string>
    {
        private SupplierRepository supplierRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public SuppliersController(MyContext myContext, SupplierRepository supplierRepository, IConfiguration configuration) : base(supplierRepository)
        {
            this.supplierRepository = supplierRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpGet("AllSupplier")]
        public ActionResult AllSupplier()
        {
            var result = supplierRepository.getAllSupplier();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }



    }
}
