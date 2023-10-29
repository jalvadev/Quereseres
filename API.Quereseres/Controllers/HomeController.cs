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
        private readonly IUserRepository _userRepository;
        private readonly IHomeRepository _homeRepository;

        public HomeController(IUserRepository userRepository, IHomeRepository homeRepository)
        {
            _userRepository = userRepository;
            _homeRepository = homeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 1 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 2 - Getting user's house.
            House userHouse = _homeRepository.GetHouseByUserId(userId);
            if (userHouse == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentran casas para este usuario." });

            // 3 - Mapping houses to DTO.
            HouseDTO houseDto = new HouseDTO
            {
                Name = userHouse.Name,
                LimitDay = (int)userHouse.LimitDay,
                UserList = userHouse.UserList.Select(u => new UserDTO { Id = u.Id, Name = u.Name, Email = u.Email}).ToList(),
                RoomList = userHouse.RoomList?.Select(r => new RoomDTO { Id = r.Id, Name = r.Name }).ToList(),
            };

            return Ok(new ComplexWrapper<HouseDTO> { Success = true, Message = "Lista de casas obtenidas", Result = houseDto });
        }

        [HttpPost]
        public IActionResult CreateHome([FromBody] HouseDTO newHouse)
        {
            // 1 - Checking mandatory fields.
            if (newHouse == null || string.IsNullOrEmpty(newHouse.Name))
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los campos son obligatorios" });

            if (newHouse.LimitDay < 0 && newHouse.LimitDay > 6)
                return BadRequest(new SimpleWrapper { Success = false, Message = "Debes elegir un día de la semana como fecha límite." });

            // 2 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });
            
            User currentUser = _userRepository.GetUserById(userId);
            if (currentUser == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 3 - Insert new House with the user owner. 
            var home = new House { Name = newHouse.Name, LimitDay = (DayOfWeek)newHouse.LimitDay, UserList = new List<User>() };
            home.UserList.Add(currentUser);
            
            home = _homeRepository.InsertHome(home);
            if (home == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la casa." });

            // 4 - Mapping house to DTO.
            HouseDTO house = new HouseDTO
            {
                Name = home.Name,
                LimitDay = (int)home.LimitDay,
                UserList = home.UserList.Select(h => new UserDTO { Id = h.Id, Name = h.Name, Email = h.Email }).ToList(),
                RoomList = home.RoomList?.Select(r => new RoomDTO { Id = r.Id, Name = r.Name }).ToList()
            };

            return Ok(new ComplexWrapper<HouseDTO> { Success = true, Message = "Casa insertada correctamente", Result = house });
        }
    }
}
