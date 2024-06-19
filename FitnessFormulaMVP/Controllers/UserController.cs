using Microsoft.AspNetCore.Mvc;

namespace FitnessFormulaMVP.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }

}
