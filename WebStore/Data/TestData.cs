
using System;
using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 30, BirthDate = new DateTime(1990, 1, 1).ToShortDateString(), DateFrom = new DateTime(2010, 1, 1).ToShortDateString() },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 40, BirthDate = new DateTime(1980, 1, 1).ToShortDateString(), DateFrom = new DateTime(2000, 1, 1).ToShortDateString() },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 50, BirthDate = new DateTime(1970, 1, 1).ToShortDateString(), DateFrom = new DateTime(2000, 1, 1).ToShortDateString() },
        };
    }
}
