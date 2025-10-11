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
            // Clear any existing session
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _userService.ValidateUser(model);
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

                // Set last activity time
                HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());

                // Debug: Check role and redirect
                Console.WriteLine($"User Role: {user.Role.Name}, Redirecting...");

                if (user.Role.Name == "Administrator")
                {
                    Console.WriteLine("Redirecting to Admin Index");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Console.WriteLine("Redirecting to FIRST questionnaire form");
                    // Get active forms and redirect to first one
                    var forms = await _formService.GetActiveFormsAsync();
                    if (forms.Any())
                    {
                        return RedirectToAction("Form", "Questionnaire", new { id = forms.First().Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No active questionnaires available.";
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
