using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        public TestMiddleware(RequestDelegate Next)
        {
            _Next = Next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //действие до след. узла в конвейере
            //context.Response
            var next = _Next(context);
            //действие во время того, как оставшаяся часть конвейера что-то делает с контекстом
            await next;// точка синхронизации
            //действие  по завершении работы с оставшейся частью конвейере
        }
    }
}
