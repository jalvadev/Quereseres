using API.Quereseres.DTOs;
using API.Quereseres.Helpers;
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
        private readonly IUserRepository _userRepository;
        private readonly IHomeRepository _homeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHouseworkRepository _houseworkRepository;

        public HouseworkController(IUserRepository userRepository, IHomeRepository homeRepository, IRoomRepository roomRepository, IHouseworkRepository houseworkRepository) 
        {
            _userRepository = userRepository;
            _homeRepository = homeRepository;
            _roomRepository = roomRepository;
            _houseworkRepository = houseworkRepository;
        }

        /// <summary>
        /// Return a list of houseworks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            // 1 - Get current user.
            int userId = JWTHelper.GetUserId(HttpContext);
            if (userId == -1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener el usuario." });

            // 2 - Check user belongs to this house.
            House userHouse = _homeRepository.GetHouseByUserId(userId);
            if (userHouse == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se encuentran casas para este usuario." });


            bool userBelongsToHouse = userHouse.UserList.Exists(u => u.Id == userId);
            if (!userBelongsToHouse)
                return BadRequest(new SimpleWrapper { Success = false, Message = "El usuario no pertenece a la casa indicada." });

            // 3 - Get list of housework.
            List<Housework> houseworkList = _houseworkRepository.ListHouswork(userHouse.Id);

            // 4 - Mapping to DTO.
            List<HouseworkDTO> houseworkDTOList = houseworkList.Select(h => new HouseworkDTO
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                RoomName = h.Room.Name,
                UserName = h.AssignedUser.Name
            }).ToList();

            return Ok(new ComplexWrapper<List<HouseworkDTO>> { Success = true, Message = "Lista de tareas obtenidas.", Result = houseworkDTOList });
        }

        [HttpPost]
        public IActionResult NewHousework([FromBody] HouseworkDTO newHousework)
        {
            // 1 - Check mandatory fields.
            if (newHousework == null || string.IsNullOrEmpty(newHousework.Name) || string.IsNullOrEmpty(newHousework.UserEmail))
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los campos nombre y email son obligatorios." });

            // 2 - Check user and home.
            var homeIsCorrect = _homeRepository.CheckHomeByIdAndUserEmail(newHousework.HouseId, newHousework.UserEmail);
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

            // 5 - Mapping to DTO.
            newHousework.Id = housework.Id;
            newHousework.UserEmail = null;
            newHousework.AssignedUser = new UserDTO
            {
                Id = housework.AssignedUser.Id,
                Name = housework.AssignedUser.Name,
                Email = housework.AssignedUser.Email
            };

            return Ok(new ComplexWrapper<HouseworkDTO> { Success = true, Message = "Tarea creada.", Result = newHousework });
        }
    }
}
