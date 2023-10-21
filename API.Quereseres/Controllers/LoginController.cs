using API.Quereseres.DTOs;
using API.Quereseres.Helpers;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using API.Quereseres.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

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
            if (LoginMandatoryFieldsEmpty(user))
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los datos de email y constraseña no pueden estar vacíos." });

            // Hashs password and get user by credentials.
            user.Password = CryptoHelper.GenerateSHA512String(user.Password);
            User fullUser = _userRepository.GetUserByCredentials(user.Email, user.Password);

            if (!checkUserOk(fullUser))
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentra el usuario." });

            string token = GenerateStringToken(fullUser);

            return Ok(new ComplexWrapper<string> { Success = true, Message = "Token obtained successfuly.", Result = token });
        }

        #region Private Methods
        private bool LoginMandatoryFieldsEmpty(UserLoginDTO user)
        {
            return string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password);
        }

        private bool checkUserOk(User user)
        {
            return user != null
                && user.Id > 0
                && !string.IsNullOrEmpty(user.Email) 
                && !string.IsNullOrEmpty(user.Name);
        }

        private string GenerateStringToken(User user)
        {
            var key = _configuration.GetValue<string>("Jwt:Key");
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            var audience = _configuration.GetValue<string>("Jwt:Audience");

            string token = JWTHelper.GenerateToken(user, key, issuer, audience);

            return token;
        }
        #endregion
    }
}
