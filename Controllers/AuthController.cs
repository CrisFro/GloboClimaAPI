using Microsoft.AspNetCore.Mvc;

namespace GloboClimaAPI.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
