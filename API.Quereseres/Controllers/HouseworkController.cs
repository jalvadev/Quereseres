using API.Quereseres.DTOs;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using API.Quereseres.Repositories;
using API.Quereseres.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Quereseres.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HouseworkController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IHomeRepository _homeRepository;
        private IRoomRepository _roomRepository;
        private IHouseworkRepository _houseworkRepository;

        public HouseworkController(IUserRepository userRepository, IHomeRepository homeRepository, IRoomRepository roomRepository, IHouseworkRepository houseworkRepository) 
        {
            _userRepository = userRepository;
            _homeRepository = homeRepository;
            _roomRepository = roomRepository;
            _houseworkRepository = houseworkRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Houseworks");
        }

        [HttpPost]
        public IActionResult NewHousework([FromBody] HouseworkDTO newHousework)
        {
            // 1 - Check mandatory fields.
            if (newHousework == null || string.IsNullOrEmpty(newHousework.Name) || String.IsNullOrEmpty(newHousework.UserEmail))
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los campos nombre y email son obligatorios." });

            // 2 - Check user and home.
            var homeIsCorrect = _homeRepository.CheckHomeByIdAndUserEmail(newHousework.HomeId, newHousework.UserEmail);
            if (!homeIsCorrect)
                return BadRequest(new SimpleWrapper { Success = false, Message = "La casa no coincide con el usuario." });

            // 3 - Check room exists.
            var room = _roomRepository.GetRoomById(newHousework.RoomId);
            if (room == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentra la habitación definida." });

            // 4 - Get user.
            var user = _userRepository.GetUserByEmail(newHousework.UserEmail);
            if (user == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentra el usuario." });

            // 4 - Create housework.
            Housework housework = new Housework
            {
                Name = newHousework.Name,
                Description = newHousework.Description,
                AssignedUser = user,
                Room = room,
            };
            housework = _houseworkRepository.InsertHousework(housework);
            if (housework == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se ha podido insertar la nueva tarea." });

            newHousework.Id = housework.Id;

            return Ok(new ComplexWrapper<HouseworkDTO> { Success = true, Message = "Tarea creada.", Result = newHousework });
        }
    }
}
