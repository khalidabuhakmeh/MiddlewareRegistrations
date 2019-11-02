using System;

namespace MiddlewareRegistrations.Middlewares.Infrastructure
{
[AttributeUsage(AttributeTargets.Class)]
public class MiddlewareAttribute : Attribute
{
    public int Order { get; set; } = int.MaxValue;
    public MiddlwareScope Scope { get; set; } = MiddlwareScope.Scoped;
}
}