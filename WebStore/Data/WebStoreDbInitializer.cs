using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Identity;
using WebStoreDAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDbInitializer(
            WebStoreDB db,
            ILogger<WebStoreDbInitializer> Logger,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager)
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация бд...");
            //_db.Database.EnsureDeleted();//для отладки
            //_db.Database.EnsureCreated();
            var db = _db.Database;
            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение миграций....");
                db.Migrate();
                _Logger.LogInformation("Выполнение миграций выполнено успешно");
            }
            else
                _Logger.LogInformation("База данных находится в актуальной версии ({0.0.0###} с)",
                    timer.Elapsed.TotalSeconds);

            try
            {
                InitializeProducts();
                InitializeIdentityAsync().Wait();
            }
            catch (Exception error)
            {
                _Logger.LogInformation(error, "Ошибка при выполнении инициализации БД");
                throw;
            }
            _Logger.LogInformation("Инициализация БД выполнена успешно! ({0.0.0###} с)",
                    timer.Elapsed.TotalSeconds);


        }
        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();
            if (_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация бд не требуется");
                return;
            }
            _Logger.LogInformation("Инициализация товаров...");
            _Logger.LogInformation("Добавление секций...");
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections); //добавление локально 

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");// перевод таблицы бд состояние ручной вставки первичного ключа
                _db.SaveChanges();//добавление в бд sql
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");// перевод таблицы бд состояние ручной вставки первичного ключа

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Секции успешно добавлены");

            _Logger.LogInformation("Добавление брендов...");
            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands); //добавление локально 

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");// перевод таблицы бд состояние ручной вставки первичного ключа
                _db.SaveChanges();//добавление в бд sql
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");// перевод таблицы бд состояние ручной вставки первичного ключа

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Бренды успешно добавлены");

            _Logger.LogInformation("Добавление товаров...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products); //добавление локально 

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");// перевод таблицы бд состояние ручной вставки первичного ключа
                _db.SaveChanges();//добавление в бд sql
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");// перевод таблицы бд состояние ручной вставки первичного ключа

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Товары успешно добавлены");
            _Logger.LogInformation("Инициализация товаров выполнена успешно! ({0.0.0###} с)",
                    timer.Elapsed.TotalSeconds);


        }

        private async Task InitializeIdentityAsync()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация системы identity....");

            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Роль {0} отсутствует, создаю...", RoleName);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Роль {0} успешно создана", RoleName);
                }
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrtator) is null)
            {
                _Logger.LogInformation("Пользователь администратор отсутствует, создаю...");
                var admin = new User
                {
                    UserName = User.Administrtator
                };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Пользователь администратор создан успешно");
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    _Logger.LogInformation("Пользователь администратор наделен ролью администратора");
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"ошибка при создании учетной записи админа:" +
                        $"{string.Join(",", errors)}");
                }
            }


            _Logger.LogInformation("Инициализация системы identity завершено успешно за {0.0.0###} с",
                 timer.Elapsed.TotalSeconds);
        }

    }
}
