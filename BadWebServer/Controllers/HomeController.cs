using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using GoodServer.Models;
using GoodServer.Data;
using System.Threading.Tasks;

namespace November2018.BadWebServer.Controllers
{
    public class HomeController : Controller
    {
        private UserManager _userManager;
        public HomeController()
        {
            _userManager = new UserManager();
        }


        public IActionResult Index()
        {
            return View();
        }

        [RequireHttps]
        public IActionResult Register()
        {
            return View();
        }


        [RequireHttps]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.CreateAsync(model.Email, model.Password))
                {
                    this.Response.Cookies.Append("Email", model.Email);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("Email", "This address is already in use");
                }
            }

            return View();
        }

        public IActionResult SignOut()
        {
            this.Response.Cookies.Delete("Email");
            return Redirect("/");
        }

        [RequireHttps]
        public IActionResult SignIn()
        {
            return View();
        }

        [RequireHttps]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.CheckPasswordAsync(model.Email, model.Password))
                {
                    this.Response.Cookies.Append("Email", model.Email);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("Email", "There was a problem with your username or password");
                }
            }
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            if (this.HttpContext.Request.Cookies.ContainsKey("email"))
            {
                string email = this.HttpContext.Request.Cookies["email"];
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    return View(user);
                }
            }
            return Redirect("/");
        }



        [RequireHttps]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {
            if (ModelState.IsValid)
            {
                bool success = await _userManager.UpdateAsync(model);
                if(success)
                {
                    this.TempData.Add("Success", "Updated Profile Successfully");
                }
                else
                {
                    this.TempData.Add("Danger", "Unable to Update Profile");
                }
            }
            return RedirectToAction("Profile");
        }

    }
}