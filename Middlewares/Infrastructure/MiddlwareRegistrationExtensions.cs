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
                switch (mw.Scope)
                {
                    case MiddlewareScope.Singleton:
                        serviceCollection.AddSingleton(mw.Type);
                        break;
                    case MiddlewareScope.Transient:
                        serviceCollection.AddTransient(mw.Type);
                        break;
                    default:
                        serviceCollection.AddScoped(mw.Type);
                        break;
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

            foreach (var mw in middlewares) applicationBuilder.UseMiddleware(mw.Type);

            return applicationBuilder;
        }

        private static List<IMiddlewareInformation> MiddlewareInformationsFromAssembly(Assembly assembly)
        {
            IMiddlewareInformation GetMiddlewareInformation(Type type)
            {
                var attribute = type.GetCustomAttribute<MiddlewareAttribute>();

                if (typeof(IMiddleware).IsAssignableFrom(type))
                {
                    if (attribute != null)
                    {
                        return new MiddlewareInformation(attribute, type);
                    }
                }

                if (typeof(IMiddlewareInformation).IsAssignableFrom(type))
                {
                    var instance = Activator.CreateInstance(type) as IMiddlewareInformation;
                    return instance;
                }

                return null;
            }

            var types = new[]
            {
                typeof(IMiddlwareRegistrar),
                typeof(IMiddleware)
            };

            var middlewares = assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(type => types.Any(t => t.IsAssignableFrom(type)))
                .Select(GetMiddlewareInformation)
                .Where(x => x != null)
                .OrderBy(x => x.Order)
                .ToList();

            return middlewares;
        }
    }
    
    
}