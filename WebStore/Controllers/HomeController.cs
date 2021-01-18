using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 30, BirthDate =new DateTime(1990,1,1).ToShortDateString(), DateFrom = new DateTime(2010, 1, 1).ToShortDateString() },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 40, BirthDate = new DateTime(1980, 1, 1).ToShortDateString(), DateFrom = new DateTime(2000, 1, 1).ToShortDateString() },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 50, BirthDate = new DateTime(1970, 1, 1).ToShortDateString(), DateFrom = new DateTime(2000, 1, 1).ToShortDateString() },
        };

        public IActionResult Index() => View(/*"SecondView"*/);

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Employees()
        {
            return View(__Employees);
        }
        public IActionResult Employee(int id)
        {

            return View((from employ in __Employees where employ.Id == id select employ).ToList()[0]);
        }
    }
}
