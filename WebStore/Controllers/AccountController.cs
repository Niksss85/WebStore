using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<AccountController> _Logger;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManger, SignInManager<User> SignInManager, ILogger<AccountController> Logger)
        {
            _UserManager = UserManger;
            _Logger = Logger;
            _SignInManager = SignInManager;
        }
        #region Register
        //отправка Html  с формой
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());
        //принятие формы обратно
        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            _Logger.LogInformation("Registration of user {0}", Model.UserName);
            var user = new User
            {
                UserName = Model.UserName,
            };
            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                _Logger.LogInformation("User {0} successfully registred", Model.UserName);

                await _UserManager.AddToRoleAsync(user, Role.Users);
                _Logger.LogInformation("User {0} get role User", Model.UserName);

                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            _Logger.LogWarning("Problems in registration of user {0}, errors: {1}", Model.UserName,
                string.Join(',', registration_result.Errors.Select(e=>e.Description)));

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }
        #endregion
        #region Login
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });
       
        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
#if DEBUG
                false
#else
                true
#endif
                );
            if (login_result.Succeeded)
            {
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Inalid username or password!");
            return View(Model);

        }
        #endregion
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

    }
}
