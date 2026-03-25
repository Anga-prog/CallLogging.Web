using CallLogging.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallLogging.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.HasClaim("IsStaff", "true"))
                    return RedirectToAction("Dashboard", "Staff");
                if (User.HasClaim("IsClient", "true"))
                    return RedirectToAction("Dashboard", "Client");
            }
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
