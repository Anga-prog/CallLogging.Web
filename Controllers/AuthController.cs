using Microsoft.AspNetCore.Mvc;

namespace CallLogging.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }



        public IActionResult Logout()
            => View();
    }
}
