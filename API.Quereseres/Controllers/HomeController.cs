using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Quereseres.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {

            return Ok("Home");
        }

        [HttpPost]
        public IActionResult CreateHome()
        {


            return Ok("CreateHome");
        }
    }
}
