using Cashier.Base;
using Cashier.Context;
using Cashier.Model;
using Cashier.Repository.Data;
using Cashier.ViewModel;
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
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private AccountRepository accountRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public AccountsController(MyContext myContext, AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpPost("Login")]
        public ActionResult<LoginVM> GetLogin(LoginVM registerVM)
        {
            var result = accountRepository.Login(registerVM);
            if (result == 2) //email salah
            {
                return NotFound();
            }
            else if (result == 3) //ChangePassword salah
            {
                return NotFound();
            }
            else if (result == 1)
            {

                var getnik = context.Users.Where(r => r.email == registerVM.email).FirstOrDefault();
                var getroleid = getnik.Account.Roleid;

                var getrole = context.Roles.Where(t => t.id == getroleid).FirstOrDefault();

                if (getrole.id == 3)
                {
                    return Ok(new { idtoken = "Unverified", Email = registerVM.email + "|" + getnik.id });
                }
                else
                {
                    var claims = new List<Claim> { };

                    claims.Add(new Claim(ClaimTypes.Email, registerVM.email));
                    claims.Add(new Claim(ClaimTypes.Role, getrole.name));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: signIn
                        );

                    var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                    return Ok(new { idtoken = idtoken, Email = registerVM.email ,idLogin = getnik.id});
                }

            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpPost("Forgot")]
        public ActionResult GetForgot(LoginVM loginVM)
        {
            var result = accountRepository.ForgotPassword(loginVM);
            switch (result)
            {
                case 1:
                    return Ok();
                case 2:
                    return NotFound();
                case 3:
                    return NotFound();
                default:
                    return BadRequest();

            }

        }


    }
}
