using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }
        //      [Route("all")]
        public IActionResult Index() => View(_EmployeesData.Get());
        //     [Route("info(id:{id})")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee is not null)
                return View(employee);
            return NotFound();
        }
        #region Edit
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var employee = _EmployeesData.Get(id);
            if (employee is null)
                return NotFound();
            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age,
                BirthDate = employee.BirthDate,
                DateFrom = employee.DateFrom
            });
        }
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.MiddleName,
                Age = model.Age,
                BirthDate = model.BirthDate,
                DateFrom = model.DateFrom
            };
            _EmployeesData.Update(employee);
            return RedirectToAction("Index");

        }
        #endregion
    }
    }
