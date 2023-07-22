using API.Quereseres.DTOs;
using API.Quereseres.Helpers;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using API.Quereseres.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Quereseres.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IHomeRepository _homeRepository;

        public HomeController(IUserRepository userRepository, IHomeRepository homeRepository)
        {
            _userRepository = userRepository;
            _homeRepository = homeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return Ok("Home");
        }

        [HttpPost]
        public IActionResult CreateHome([FromBody] InsertHouseDTO newHouse)
        {
            // 1 - Checking mandatory fields.
            if (newHouse == null || string.IsNullOrEmpty(newHouse.Name))
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los campos son obligatorios" });

            if (newHouse.RecordInitDate <= DateTime.Now)
                return BadRequest(new SimpleWrapper { Success = false, Message = "La fecha no puede ser hoy ni un día anterior." });

            // 2 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });
            
            User currentUser = _userRepository.GetUserById(userId);

            // 3 - Insert new House with the user owner.
            var home = new Home { Name = newHouse.Name, RecordInitDate = newHouse.RecordInitDate, UserList = new List<User>() };
            home.UserList.Add(currentUser);
            
            home = _homeRepository.InsertHome(home);
            if (home == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la casa." });

            return Ok(new ComplexWrapper<Home> { Success = true, Message = "Casa insertada correctamente", Result = home });
        }
    }
}
