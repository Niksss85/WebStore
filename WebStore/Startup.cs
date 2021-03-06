﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.Data;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Services;
using WebStore.Infrastructure.Services.InMemory;
using WebStore.Infrastructure.Services.InSQL;
using WebStore.Models;
using WebStore.Models.InCookies;
using WebStoreDAL.Context;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {

        public void ConfigureServices(IServiceCollection services)
        {
            var connection_string_name = Configuration["ConnectionString"];
            switch (connection_string_name)
            {
                default: throw new InvalidOperationException($"Подключение {connection_string_name} не поддерживается");
                case "SqlServer":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlServer(Configuration.GetConnectionString(connection_string_name))
                           .UseLazyLoadingProxies());
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreDB>(opt =>
                    opt.UseSqlite(Configuration.GetConnectionString(connection_string_name), o => o.MigrationsAssembly("WebStore.Dal.Sqlite")));
                    break;
            }


            services.AddTransient<WebStoreDbInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);


            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.SlidingExpiration = true;//новый идентификатор сеанса при регистрации
            }
            );

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            //services.AddTransient<IProductData, InMemoryProductData>();
            services.AddTransient<IProductData, SqlProductData>();
            services.AddTransient<ICartService, InCookiesCartService>();
            services.AddTransient<IOrderService, SqlOrderService>();

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db)
        {
            db.Initialize();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //обработка исключений
                app.UseBrowserLink();//отладка vs studio
            }

            app.UseStaticFiles(); //проверка на запрос файла -> файл отправляется
                                  //в браузер
            app.UseRouting(); //извлечение информации о маршрутах

            app.UseAuthentication(); //извлекается из запроса(cookies) информацию о пользователе
            app.UseAuthorization(); // проверка доступа к ресурсу

            app.UseWelcomePage("/welcome");//промеж. по

            app.UseMiddleware<TestMiddleware>();//регистрация пром. по
            app.MapWhen(
                context => context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == "5",
                context => context.Run(async request => await request.Response.WriteAsync("Hello id 5")));

            app.Map("/hello", context => context.Run(async request => await request.Response.WriteAsync("Hello!!")));
            app.UseEndpoints(endpoints => //срабатывает маршрут
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });
                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                // http://localhost:5000 -> controller = "Home" action = "Index" параметр = null
                // http://localhost:5000/Catalog/Products/5 controller = "Catalog" action = "Products" параметр = 5
            });
        }
    }
}
