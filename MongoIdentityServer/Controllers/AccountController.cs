﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MongoIdentityServer.Models;

namespace MongoIdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Required][EmailAddress] string email, [Required] string password, string returnurl)
        {
            if (!ModelState.IsValid)
                return View();

            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                var result = await signInManager.PasswordSignInAsync(appUser, password, false, false);


                if (result.Succeeded)
                {
                    return Redirect(returnurl ?? "/");
                }
            }
            ModelState.AddModelError(nameof(email), "Login Failed: Invalid Email or Password");

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
