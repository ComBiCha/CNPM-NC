﻿using E_commerce.Models;
using E_commerce.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
                if(result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View(loginVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Create(UserModel user)
		{
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName= user.UserName, Email= user.Email};
                IdentityResult result = await _userManager.CreateAsync(newUser,user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Account created successfully!";
                    return Redirect("/Account/Login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
			return View(user);
		}
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
	}
}
