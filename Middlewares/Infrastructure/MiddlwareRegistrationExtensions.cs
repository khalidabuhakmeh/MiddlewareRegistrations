using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MiddlewareRegistrations.Middlewares.Infrastructure
{
public static class MiddlewareRegistrationExtensions
{
    public static IServiceCollection AddMiddlewares<T>(this IServiceCollection serviceCollection)
    {
        return AddMiddlewares(serviceCollection, typeof(T).Assembly);
    }
    
    public static IServiceCollection AddMiddlewares(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var middlewares = MiddlewareInformationsFromAssembly(assembly);
        foreach (var mw in middlewares)
        {
            switch (mw.Scope)
            {
                case MiddlwareScope.Singleton:
                    serviceCollection.AddSingleton(mw.Type);
                    break;
                case MiddlwareScope.Transient:
                    serviceCollection.AddTransient(mw.Type);
                    break;
                default:
                    serviceCollection.AddScoped(mw.Type);
                    break;
            }
            
        }
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseMiddlewares<T>(this IApplicationBuilder applicationBuilder)
    {
        return UseMiddlewares(applicationBuilder, typeof(T).Assembly);
    }
    
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder, Assembly assembly)
    {
        var middlewares = MiddlewareInformationsFromAssembly(assembly);

        foreach (var mw in middlewares)
        {
            applicationBuilder.UseMiddleware(mw.Type);
        }
        
        return applicationBuilder;
    }

    private static List<MiddlewareInformation> MiddlewareInformationsFromAssembly(Assembly assembly)
    {
        MiddlewareInformation GetMiddlewareInformation(Type type)
        {
            var attribute = type.GetCustomAttribute<MiddlewareAttribute>();

            return new MiddlewareInformation
            {
                Attribute = attribute,
                Order = attribute?.Order ?? -1,
                Type = type,
                Scope = attribute?.Scope ?? MiddlwareScope.Scoped
            };
        } 
        
        var middlewares = assembly
            .GetTypes()
            .Where(x => typeof(IMiddleware).IsAssignableFrom(x))
            .Select(GetMiddlewareInformation)
            .Where(x => x.HasAttribute)
            .OrderBy(x => x.Order)
            .ToList();
        
        return middlewares;
    }

    private class MiddlewareInformation
    {
        public MiddlewareAttribute Attribute { get; set; } 
        public Type Type { get; set; }
        public int Order { get; set; }
        public bool HasAttribute => Attribute != null;
        
        public MiddlwareScope Scope { get; set; } 
    }
}
}