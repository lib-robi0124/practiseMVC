using Microsoft.AspNetCore.Mvc;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserCredentialsViewModel());
        }

        [HttpPost]
        public IActionResult Login(UserCredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
                return View(credentials);

            var user = _userService.Login(credentials);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View(credentials);
            }

            // Store in session
            HttpContext.Session.SetString("UserId", user.CompanyId.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Questionnaire");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
