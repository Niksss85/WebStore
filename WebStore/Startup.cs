using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //обработка исключений
            }

            app.UseStaticFiles(); //проверка на запрос файла -> файл отправляется
                                  //в браузер
            app.UseRouting(); //извлечение информации о маршрутах

            app.UseEndpoints(endpoints => //срабатывает маршрут
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                // http://localhost:5000 -> controller = "Home" action = "Index" параметр = null
                // http://localhost:5000/Catalog/Products/5 controller = "Catalog" action = "Products" параметр = 5
            });
        }
    }
}
