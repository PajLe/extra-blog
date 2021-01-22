using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExtraBlog.Auth;
using ExtraBlog.DTOs;
using ExtraBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExtraBlog.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase 
    {
        private readonly IAuthentication _auth;
        private readonly IConfiguration _config;

        public AuthenticationController(IAuthentication auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            if (await _auth.UserExists(userForRegisterDTO.Username))
                return BadRequest("Username already exists");
            
            var user = new User {
                Username = userForRegisterDTO.Username
            };

            var registeredUser = await _auth.Register(user, userForRegisterDTO.Password, null);

            return Ok(200);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userToLogin = await _auth.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

            if (userToLogin == null)
                return Unauthorized("Error while logging in");

            var token = GenerateToken(userToLogin);

            return Ok(new
            {
                token = token
            });
        }

        private string GenerateToken(User authenticatedUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, authenticatedUser.Username), //username korisnika laza123
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(createdToken);
        }
    }
}
