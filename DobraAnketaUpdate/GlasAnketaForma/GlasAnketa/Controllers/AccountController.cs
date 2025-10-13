using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IQuestionFormService _formService;

        public AccountController(IUserService userService, IQuestionFormService formService)
        {
            _userService = userService;
            _formService = formService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.ValidateUser(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Company ID or Password");
                return View(model);
            }
            // Store user in session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.Name);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetInt32("CompanyId", user.CompanyId);
            HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());

            if (user.Role.Name == "Administrator")
                return RedirectToAction("Index", "Admin");

            var form = await _formService.GetActiveFormAsync();
            if (form != null)
            {
                return RedirectToAction("ShowForm", "Questionnaire", new { formId = 1 });
            }
            else
            {
                TempData["ErrorMessage"] = "No active questionnaires available.";
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
