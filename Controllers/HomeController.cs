using CallLogging.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CallLogging.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
             => View();
    }
}
