using CodingChallenge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CodingChallenge.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ShopSiloApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AauthController : ControllerBase
    {
        IConfiguration _config;
        private readonly TaskDBcontext _context;
        public AauthController(IConfiguration configuration, TaskDBcontext context)
        {
            this._config = configuration;
            _context = context;
        }

        [NonAction]
        public User Validate(string username, string password)
        {

            User s = _context.Users
                .FirstOrDefault(i => i.Username == username && i.Password == password);

            return s;

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth(string UserName, string UserPass)
        {
            IActionResult response = Unauthorized();

            var s = Validate(UserName, UserPass);
            if (s != null)
            {

                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha512Signature);

                var subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, s.Username),
                    new Claim(JwtRegisteredClaimNames.Email,s.Email),
                    new Claim(ClaimTypes.Role, s.Role.ToString()) // Assign role to the token
                    });

                var expires = DateTime.UtcNow.AddMinutes(1600);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddMinutes(1600),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(jwtToken);

            }
            return response;
        }

    }
}