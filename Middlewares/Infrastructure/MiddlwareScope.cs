namespace MiddlewareRegistrations.Middlewares.Infrastructure
{
public enum MiddlwareScope 
{
    /// <summary>
    /// Scoped to the current request
    /// </summary>
    Scoped = 0,
    /// <summary>
    /// The middleware will be created
    /// every time it is required
    /// </summary>
    Transient = 1,
    /// <summary>
    /// singleton middleware should be used
    /// carefully, as it can capture dependencies
    /// that should be transient or scoped
    /// </summary>
    Singleton = 2
}
}