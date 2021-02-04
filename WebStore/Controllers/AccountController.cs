﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManger, SignInManager<User> SignInManager)
        {
            _UserManager = UserManger;
            _SignInManager = SignInManager;
        }
        //отправка Html  с формой
        public IActionResult Register() => View(new RegisterUserViewModel());
        //принятие формы обратно
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var user = new User
            {
                UserName = Model.UserName,
            };
            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("",error.Description);

            return View(Model);
        }
    }
}
