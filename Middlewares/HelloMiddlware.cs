using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiddlewareRegistrations.Middlewares.Infrastructure;

namespace MiddlewareRegistrations.Middlewares
{
[Middleware(Order = 1)]
public class HelloMiddleware : IMiddleware
{
    private readonly ILogger<HelloMiddleware> logger;

    public HelloMiddleware(ILogger<HelloMiddleware> logger)
    {
        this.logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        logger.LogInformation("Hello, ");
        await next(context);
    }
}
}