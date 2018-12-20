using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using GoodServer.Models;
using GoodServer.Services;
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
    }
}