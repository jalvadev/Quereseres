using API.Quereseres.DTOs;
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
    public class HouseworkWeeklyController : ControllerBase
    {
        private readonly IHouseworkWeeklyRepository _houseworkWeeklyRepository;
        private readonly IHouseworkRepository _houseworkRepository;
        private readonly IHomeRepository _homeRepository;
        public HouseworkWeeklyController(IHouseworkWeeklyRepository houseworkWeeklyRepository, IHouseworkRepository houseworkRepository, IHomeRepository homeRepository)
        {
            _houseworkWeeklyRepository = houseworkWeeklyRepository;
            _houseworkRepository = houseworkRepository;
            _homeRepository = homeRepository;
        }

        [HttpGet("/{houseworkId}")]
        public IActionResult Detail(int houseworkId)
        {
            // 1 - Getting housework by Id.
            var houseworkWeekly = _houseworkWeeklyRepository.GetHouseworkWeeklyById(houseworkId);
            if (houseworkWeekly == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener detalle de la tarea." });

            // 2 - Mapping to DTO.
            HouseworkWeeklyDTO houseworkWeeklyDTO = new HouseworkWeeklyDTO
            {
                Id = houseworkWeekly.Id,
                Name = houseworkWeekly.Housework.Name,
                UserName = houseworkWeekly.Housework.AssignedUser.Name,
                RoomName = houseworkWeekly.Housework.Room.Name,
                IsDone = houseworkWeekly.IsDone
            };

            return Ok(new ComplexWrapper<HouseworkWeeklyDTO> { Success = false, Message = "Detalle de la tarea obtenido.", Result = houseworkWeeklyDTO });
        }

        [HttpGet("/User/{userId}")]
        public IActionResult Index(int userId)
        {
            // 1 - Getting housework by Id.
            var houseworkWeeklyList = _houseworkWeeklyRepository.GetHouseworkWeeklyByUserId(userId);
            if (houseworkWeeklyList == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la lista de tareas del usuario." });

            // 2 - Mapping to DTO.
            List<HouseworkWeeklyDTO> houseworkWeeklyDTOList = houseworkWeeklyList.Select(h => new HouseworkWeeklyDTO
            {
                Id = h.Id,
                Name = h.Housework.Name,
                UserName = h.Housework.AssignedUser.Name,
                RoomName = h.Housework.Room.Name,
                IsDone = h.IsDone
            }).ToList();

            return Ok(new ComplexWrapper<List<HouseworkWeeklyDTO>> { Success = false, Message = "Lista de tareas por usuario obtenida.", Result = houseworkWeeklyDTOList });
        }

        [HttpGet("/Room/{roomId}")]
        public IActionResult ListByRoom(int roomId)
        {
            // 1 - Getting housework by Id.
            var houseworkWeeklyList = _houseworkWeeklyRepository.GetHouseworkWeeklyByRoomId(roomId);
            if (houseworkWeeklyList == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la lista de tareas de la habitación." });

            // 2 - Mapping to DTO.
            List<HouseworkWeeklyDTO> houseworkWeeklyDTOList = houseworkWeeklyList.Select(h => new HouseworkWeeklyDTO
            {
                Id = h.Id,
                Name = h.Housework.Name,
                UserName = h.Housework.AssignedUser.Name,
                RoomName = h.Housework.Room.Name,
                IsDone = h.IsDone
            }).ToList();

            return Ok(new ComplexWrapper<List<HouseworkWeeklyDTO>> { Success = false, Message = "Lista de tareas de la habitación obtenida.", Result = houseworkWeeklyDTOList });
        }

        [HttpPost]
        public IActionResult CreateHouseworkWeekly([FromBody] HouseworkWeeklyDTO dto)
        {
            // 1 - Check mandatory fields.
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.RoomName) || dto.UserId < 1)
                return BadRequest(new SimpleWrapper { Success = false, Message = "Los campos nombre, habitación y usuario son obligatorios" });

            // 2 - Get housework.
            Housework housework = _houseworkRepository.GetHouseworkById(dto.houseworkId);
            if (housework == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se puede obtener la tarea indicada." });

            // 3 - Get home and getting next date.
            House house = _homeRepository.GetHouseByUserId(dto.UserId);
            var limitDay = house.LimitDay;
            DateTime today = DateTime.Today;

            int daysToAdd = ((int) limitDay - (int)today.DayOfWeek + 7) % 7;
            DateTime limitDate = today.AddDays(daysToAdd);

            // 4 - Insert new houseworkWeekly.
            HouseworkWeekly houseworkWeekly = new HouseworkWeekly { Housework = housework, LimitDate =  limitDate, IsDone = false };
            houseworkWeekly = _houseworkWeeklyRepository.CreateHouseworkWeekly(houseworkWeekly);
            if(houseworkWeekly == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo insertar la tarea." });

            // 5 - Mapping to DTO.
            dto.Id = houseworkWeekly.Id;

            return Ok(new ComplexWrapper<HouseworkWeeklyDTO> { Success = true, Message = "Tarea creada correctamente.", Result = dto});
        }

        [HttpPost("/{houseId}")]
        public IActionResult CreateAllHouseworkWeekly(int houseId)
        {
            // 1 - Getting house by id.
            House house = _homeRepository.GetHouseById(houseId);
            if (house == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo obtener la casa." });

            // 2 - Getting the limit date.
            var limitDay = house.LimitDay;
            DateTime today = DateTime.Today;

            int daysToAdd = ((int)limitDay - (int)today.DayOfWeek + 7) % 7;
            DateTime limitDate = today.AddDays(daysToAdd);

            // 3 - Creating all the houseworks for the week.
            List<HouseworkWeekly> houseworkWeeklyList = _houseworkWeeklyRepository.CreateAllHouseworkWeeklyByHouseId(houseId, limitDate);
            if (houseworkWeeklyList == null)
                return BadRequest(new SimpleWrapper { Success = false, Message = "No se pudo crear la lista de tareas." });

            // 4 - Mapping DTO.
            List<HouseworkWeeklyDTO> houseworkWeeklyDTOs = houseworkWeeklyList.Select(h => new HouseworkWeeklyDTO
            {
                Id = h.Id,
                LimitDate = h.LimitDate.Date,
                Name = h.Housework.Name,
            }).ToList();

            return Ok(new ComplexWrapper<List<HouseworkWeeklyDTO>> { Success = true, Message = "Lista de tareas creadas con éxito.", Result = houseworkWeeklyDTOs });
        }
    }
}
