using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quereseres.Helpers;
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

            // 1 - Checking mandatory fields.
            if (email == null || password == null)
            {
                ViewBag.LoginError = "Los datos de email y constraseña no pueden estar vacíos.";
                return View("Index");
            }

            // 2 - Hashs password and get user by credentials.
            password = CryptoHelper.GenerateSHA512String(password);
            User fullUser = _userRepository.GetUserByCredentials(email, password);

            // 3 - Validate user.
            if (fullUser == null)
            {
                ViewBag.LoginError = "No se encuentra el usuario.";
                return View("Index");
            }

            return Redirect("/Home");
        }
    }
}
