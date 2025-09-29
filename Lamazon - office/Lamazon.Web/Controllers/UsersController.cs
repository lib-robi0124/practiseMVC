using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;
using Lamazon.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lamazon.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        { 
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsViewModel userCredentialsViewModel, string returnUrl)
        {
            if(string.IsNullOrEmpty(userCredentialsViewModel.Email))
            {
                ModelState.AddModelError("UserLoginError", "Email is invalid and required");
                return View(userCredentialsViewModel);
            }
            var userValidationResult = _userService.ValidateUser(userCredentialsViewModel);
            if(userValidationResult is null)
            {
                ModelState.AddModelError("UserLoginError", "User not found");
                return View(userCredentialsViewModel);
            }
            //sign in user
            await AuthHelper.SignInUser(userValidationResult, HttpContext);
            if(string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("/");
            }
            return Redirect(returnUrl);

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            
            var registeredUser = _userService.RegisterUser(registerUserViewModel);
            if(registeredUser != null)
            {
                await AuthHelper.SignInUser(registeredUser, HttpContext);
                return Redirect("/");
            }
            return View(registerUserViewModel);
        }
        public async Task<IActionResult> Logout()
        {
            await AuthHelper.SignOutUser(HttpContext);
            return Redirect("/"); //redirect to home page (root page)
        }

    }
}
