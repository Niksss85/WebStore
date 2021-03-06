﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Identity;

namespace WebStore.Controllers
{
    //[Route("staff")]
    [Authorize]
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
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            if (id <= 0) return BadRequest();
            
            var employee = _EmployeesData.Get((int)id);
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
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (model.Name == "Усама" && model.MiddleName == "бен" && model.LastName == "Ладен")
                ModelState.AddModelError("", "Террористов не берём!");

            if (!ModelState.IsValid) return View(model);

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
            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else 
                _EmployeesData.Update(employee);
            return RedirectToAction("Index");

        }
        #endregion
        #region Delete
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
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
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }

        #endregion
        public IActionResult Create() => View("Edit", new EmployeeViewModel());
    }
}
