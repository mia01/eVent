using eventapp.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eventapp.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IConfiguration Configuration { get; set; }
        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpPost, Route("login")]
        public IActionResult login([FromBody]Login userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid login request");
            }

            if (userLogin.Username == "johndoe@test.com" && userLogin.Password == "def@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:IssuerSigningKey"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: Configuration["Jwt:ValidIssuer"],
                    audience: Configuration["Jwt:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}