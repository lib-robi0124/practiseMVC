using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

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
        HttpContext.Session.Clear();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserCredentialsVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var user = await _userService.ValidateUser(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Company ID or Password");
                return View(model);
            }

            // ✅ Store user info in session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role?.Name ?? "Employee");
            HttpContext.Session.SetString("UserName", user.FullName ?? "Unknown");
            HttpContext.Session.SetInt32("CompanyId", user.CompanyId);

            Console.WriteLine($"✅ Login success: {user.FullName}, Role: {user.Role?.Name}");

            // ✅ Admin redirect
            if (user.Role?.Name == "Administrator")
                return RedirectToAction("Index", "Admin");

            // ✅ Employee redirect — get active form
            var activeForm = await _formService.GetActiveFormAsync();
            if (activeForm == null)
            {
                TempData["ErrorMessage"] = "❌ No active questionnaires found.";
                return RedirectToAction("Login");
            }

            // ✅ Redirect employee to questionnaire form
            return RedirectToAction("Form", "Questionnaire", new { id = activeForm.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Login error: {ex.Message}");
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
    public IActionResult AccessDenied() => View();
}
