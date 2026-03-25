using System.Security.Claims;
using CallLogging.Services.Interfaces;
using CallLogging.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CallLogging.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Home/Index.cshtml", model);

            var user =  _userService.ValidateUserAsync(model.Email, model.Password).GetAwaiter().GetResult();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View("~/Views/Home/Index.cshtml", model);
            }

             _userService.UpdateLastLoginAsync(user.PersonId).GetAwaiter().GetResult();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.PersonId.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new("IsStaff", user.IsStaff ? "true" : "false"),
                new("IsClient", user.IsClient ? "true" : "false")
            };

            if (user.IsStaff)
            {
                claims.Add(new(ClaimTypes.Role, "Staff"));
                claims.Add(new("SystemUserId", user.SystemUserId?.ToString() ?? ""));
                claims.Add(new("RoleName", user.RoleName ?? ""));
            }

            if (user.IsClient)
            {
                claims.Add(new(ClaimTypes.Role, "Client"));
                claims.Add(new("ClientProfileId", user.ClientProfileId?.ToString() ?? ""));
                claims.Add(new("Company", user.Company ?? ""));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

             HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps).GetAwaiter().GetResult();

            if (user.IsStaff)
                return RedirectToAction("Dashboard", "Staff");

            if (user.IsClient)
                return RedirectToAction("Dashboard", "Client");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
