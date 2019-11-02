using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiddlewareRegistrations.Middlewares.Infrastructure;

namespace MiddlewareRegistrations.Middlewares
{
[Middleware(Order = 2)]
public class WorldMiddleware : IMiddleware
{
    private readonly ILogger<WorldMiddleware> logger;

    public WorldMiddleware(ILogger<WorldMiddleware> logger)
    {
        this.logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        logger.LogInformation(" World!");
        await next(context);
    }
}
}