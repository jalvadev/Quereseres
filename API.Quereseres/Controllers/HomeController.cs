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
        private IRoomRepository _roomRepository;

        public HomeController(IUserRepository userRepository, IHomeRepository homeRepository, IRoomRepository roomRepository)
        {
            _userRepository = userRepository;
            _homeRepository = homeRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 1 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 2 - Obtenemos casas para el usuario.
            List<Home> userHomes = _homeRepository.GetUserHomes(userId);
            if (userHomes == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentran casas para este usuario." });

            return Ok(new ComplexWrapper<List<Home>> { Success = true, Message = "Lista de casas obtenidas", Result = userHomes });
        }

        [HttpPost]
        public IActionResult CreateHome([FromBody] HomeDTO newHouse)
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
            if (currentUser == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 3 - Insert new House with the user owner. 
            var home = new Home { Name = newHouse.Name, RecordInitDate = newHouse.RecordInitDate, UserList = new List<User>() };
            home.UserList.Add(currentUser);
            
            home = _homeRepository.InsertHome(home);
            if (home == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la casa." });

            return Ok(new ComplexWrapper<Home> { Success = true, Message = "Casa insertada correctamente", Result = home });
        }

        [HttpPost("NewRoom")]
        public IActionResult AddRoomToHome([FromBody] RoomDTO newRoom, int homeId)
        {
            // 1 - Checking mandatory fields.
            if (string.IsNullOrEmpty(newRoom.Name))
                return BadRequest(new SimpleWrapper { Success = false, Message = "El campo nombre es obligatorio." });

            // 2 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            User currentUser = _userRepository.GetUserById(userId);
            if (currentUser == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 3 - Get home by homeId and user.
            Home home = _homeRepository.GetHomeByIdAndUser(homeId, currentUser);
            if (home == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la casa o no pertenece a este usuario."});

            if (home.RoomList == null)
                home.RoomList = new List<Room>();

            // 4 - Insert new room.
            Room room = new Room() { Name = newRoom.Name };
            home.RoomList.Add(room);

            room = _roomRepository.InsertRoom(room);
            if (room == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la habitación" });

            return Ok(new ComplexWrapper<Room> { Success = true, Message = "Habitación creada.", Result = room });
        }
    }
}
