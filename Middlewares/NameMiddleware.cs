using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiddlewareRegistrations.Middlewares.Infrastructure;

namespace MiddlewareRegistrations.Middlewares
{
    public class NameMiddleware : IMiddleware
    {
        public class NameRegistrar : MiddlewareRegistrar<NameMiddleware>
        {
            public NameRegistrar()
            {
                Order = 3;
            }
        }
        
        private readonly ILogger<NameMiddleware> logger;

        public NameMiddleware(ILogger<NameMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("John Wick!");
            await next(context);
        }
    }
}