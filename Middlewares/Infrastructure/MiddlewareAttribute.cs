using System;

namespace MiddlewareRegistrations.Middlewares.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MiddlewareAttribute : Attribute
    {
        public int Order { get; set; } = int.MaxValue;
        public MiddlewareScope Scope { get; set; } = MiddlewareScope.Scoped;
    }

    public interface IMiddlewareInformation
    {
        int Order { get; }
        MiddlewareScope Scope { get; }
        
        Type Type { get; }
    }

    public class MiddlewareInformation : IMiddlewareInformation
    {
        public MiddlewareInformation(MiddlewareAttribute attribute, Type type)
        {
            Order = attribute.Order;
            Type = type;
            Scope = attribute.Scope;
        }
        
        public int Order { get; set; }
        public MiddlewareScope Scope { get; set; }
        public Type Type { get; set; }
    }
}