using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HamedStack.AspNetCore.Endpoint;

/// <summary>
/// Provides extension methods for registering and mapping minimal API endpoints
/// derived from <see cref="MinimalApiEndpointBase"/> in an ASP.NET Core application.
/// </summary>
public static class MinimalApiEndpointsExtensions
{
    /// <summary>
    /// Registers all classes that derive from <see cref="MinimalApiEndpointBase"/> as services
    /// in the application's dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> instance for chaining additional method calls.
    /// </returns>
    /// <remarks>
    /// This method scans all loaded assemblies for non-abstract classes that derive from
    /// <see cref="MinimalApiEndpointBase"/> and registers them as transient services.
    /// </remarks>
    public static IServiceCollection AddMinimalApiEndpoints(this IServiceCollection services)
    {
        var endpointTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(MinimalApiEndpointBase)) && !type.IsAbstract);

        foreach (var type in endpointTypes)
        {
            services.AddTransient(typeof(MinimalApiEndpointBase), type);
        }

        return services;
    }

    /// <summary>
    /// Maps all registered minimal API endpoints to the provided <see cref="WebApplication"/>,
    /// enabling their routes and handlers.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoints to.</param>
    /// <returns>
    /// The <see cref="WebApplication"/> instance for chaining additional method calls.
    /// </returns>
    /// <remarks>
    /// This method resolves all registered instances of <see cref="MinimalApiEndpointBase"/>
    /// from the application's service container. Each instance is initialized with the
    /// <see cref="WebApplication"/> context and invokes its <see cref="MinimalApiEndpointBase.HandleEndpoint"/>
    /// method to define API routes and handlers.
    /// </remarks>
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<MinimalApiEndpointBase>();
        foreach (var endpoint in endpoints)
        {
            endpoint.Initialize(app);
            endpoint.HandleEndpoint(app);
        }

        return app;
    }
}
