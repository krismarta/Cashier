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
    public class CategoryController : BaseController<Category, CategoryRepository, int>
    {
        private CategoryRepository categoryRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public CategoryController(MyContext myContext, CategoryRepository categoryRepository, IConfiguration configuration) : base(categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpGet("Allcategory")]
        public ActionResult Allcategory()
        {
            var result = categoryRepository.getAllSCategory();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }



    }
}
