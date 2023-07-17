using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quereseres.Interfaces;
using Quereseres.Models;

namespace Quereseres.Controllers
{
    public class LoginController : Controller
    {
        private IUserRepository _userRepository;
        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([FromForm] string email, string password)
        {

            User fullUser = _userRepository.GetUserByCredentials(email, password);

            return Redirect("/Home");
        }
    }
}
