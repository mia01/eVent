using eventapp.Areas.Identity.Data;
using eventapp.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace eventapp.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IConfiguration Configuration { get; set; }
        private readonly SignInManager<EventAppUser> _signInManager;
        private readonly UserManager<EventAppUser> _userManager;

        public AuthController(
            UserManager<EventAppUser> userManager,
            SignInManager<EventAppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }
        [HttpPost, Route("login")]
        public async System.Threading.Tasks.Task<IActionResult> loginAsync([FromBody]Login userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid login request");
            }

            var result = await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == userLogin.Username);
                return Ok(new { Token = GenerateJwtToken(userLogin.Username, appUser) });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateJwtToken(string email, EventAppUser appUser)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:IssuerSigningKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: Configuration["Jwt:ValidIssuer"],
                audience: Configuration["Jwt:ValidAudience"],
                claims: new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(ClaimTypes.NameIdentifier, appUser.Id)
                },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}