using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManger;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(UserManager<User> UserManger, RoleManager<Role> RoleManager)
        {
            _userManger = UserManger;
            _roleManager = RoleManager;
        }
    }
}
