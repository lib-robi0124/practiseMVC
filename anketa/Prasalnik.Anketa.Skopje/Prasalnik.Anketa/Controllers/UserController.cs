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

        //[HttpPost]
        //public IActionResult Login(UserCredentialsViewModel credentials)
        //{
        //    if (!ModelState.IsValid)
        //        return View(credentials);

        //    var user = _userService.Login(credentials.CompanyId, credentials.FullName);
        //    if (user == null)
        //    {
        //        ViewBag.Error = "Invalid credentials";
        //        return View(credentials);
        //    }

        //    if (user is not UserCredentialsViewModel typedUser)
        //    {
        //        ViewBag.Error = "Invalid user type";
        //        return View(credentials);
        //    }

        //    // Store in session
        //    HttpContext.Session.SetString("UserId", typedUser.CompanyId.ToString());
        //    HttpContext.Session.SetString("UserName", typedUser.FullName);

        //    return RedirectToAction("Index", "Questionnaire");
        //}
        //public IActionResult Login(UserCredentialsViewModel credentials)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(credentials);
        //    }

        //    var userVm = _userService.Login(credentials);

        //    if (userVm == null)
        //    {
        //        ViewBag.Error = "Invalid credentials";
        //        return View(credentials);
        //    }

        //    // Use VM data in session
        //    HttpContext.Session.SetString("UserId", userVm.Id.ToString());
        //    HttpContext.Session.SetString("UserName", userVm.FullName);
        //    HttpContext.Session.SetString("Role", userVm.Role);

        //    return RedirectToAction("Index", "Questionnaire");
        //}
        [HttpPost]
        public IActionResult Login(UserCredentialsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = _userService.Login(model); // now takes UserCredentialsViewModel
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login credentials");
                    return View(model);
                }

                // Redirect to Questionnaire page after login
                return RedirectToAction("Index", "Questionnaire");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
