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
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHomeRepository _homeRepository;
        private readonly IUserRepository _userRepository;

        public RoomController(IRoomRepository roomRepository, IHomeRepository homeRepository, IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _homeRepository = homeRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get list of Rooms.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(int houseId)
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

            // 3 - Get list of Rooms by houseId.
            List<Room> roomList = _roomRepository.GetRoomListByHouseId(houseId);

            // 4 - Mapping model.
            List<RoomDTO> roomDtoList = roomList.Select(r => new RoomDTO
            {
                Id = r.Id,
                Name = r.Name,
                HouseId = houseId,
            }).ToList();

            return Ok(new ComplexWrapper<List<RoomDTO>> { Success = true, Message = "Habitaciones obtenidas con éxito.", Result = roomDtoList });
        }

        /// <summary>
        /// Get a specific Room by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("RoomDetail/{roomId}")]
        public IActionResult RoomDetail(int id)
        {
            // 1 - Getting Room by ID.
            Room room = _roomRepository.GetRoomById(id);
            if (room == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la habitación."});

            // 2 - Mapping Room to DTO.
            RoomDTO roomDto = new RoomDTO
            {
                Id = id,
                Name = room.Name,
                HouseId = room.House.Id,
                HouseworkList = room.HouseworkList?.Select(h => new HouseworkDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    AssignedUser = new UserDTO
                    {
                        Id = h.AssignedUser.Id,
                        Name = h.AssignedUser.Name,
                        Email = h.AssignedUser.Email,
                    }
                }).ToList()
            };

            return Ok(new ComplexWrapper<RoomDTO> { Success = true, Message = "Habitación obtenida correctamente.", Result = roomDto });
        }

        [HttpPost("InsertRoom")]
        public IActionResult InsertRoom([FromBody] RoomDTO newRoom)
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
            House home = _homeRepository.GetHomeByIdAndUser(newRoom.HouseId, currentUser);
            if (home == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la casa o no pertenece a este usuario." });

            if (home.RoomList == null)
                home.RoomList = new List<Room>();

            // 4 - Insert new room.
            Room room = new Room() { Name = newRoom.Name };
            home.RoomList.Add(room);

            room = _roomRepository.InsertRoom(room);
            if (room == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la habitación" });

            // 5 - Mapping to DTO.
            RoomDTO roomDto = new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                HouseId = room.House.Id,
            };

            return Ok(new ComplexWrapper<RoomDTO> { Success = true, Message = "Habitación creada.", Result = roomDto });
        }
    }
}
