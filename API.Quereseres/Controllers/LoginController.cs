using API.Quereseres.DTOs;
using API.Quereseres.Helpers;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Quereseres.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDTO user)
        {
            string errorMessage;

            // 1 - Checking mandatory fields.
            if (user.Email == null || user.Password == null)
            {
                errorMessage = "Los datos de email y constraseña no pueden estar vacíos.";
                return BadRequest(errorMessage);
            }

            // 2 - Hashs password and get user by credentials.
            user.Password = CryptoHelper.GenerateSHA512String(user.Password);
            User fullUser = _userRepository.GetUserByCredentials(user.Email, user.Password);

            // 3 - Validate user.
            if (fullUser == null)
            {
                errorMessage = "No se encuentra el usuario.";
                return BadRequest(errorMessage);
            }

            // 4 - Generate the JWT.
            var key = _configuration.GetValue<string>("Jwt:Key");
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            var audience = _configuration.GetValue<string>("Jwt:Audience");

            string token = JWTHelper.GenerateToken(fullUser, key, issuer, audience);

            return Ok(token);
        }
    }
}
