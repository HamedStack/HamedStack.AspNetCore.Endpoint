using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HamedStack.AspNetCore.Endpoint;


/// <summary>
/// Provides extension methods for registering and mapping minimal API endpoints.
/// </summary>
public static class MinimalApiEndpointsExtensions
{
    /// <summary>
    /// Registers all minimal API endpoint implementations found in the application into the service collection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddMinimalApiEndpoints(this IServiceCollection services)
    {
        var endpointTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IMinimalApiEndpoint).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        foreach (var type in endpointTypes)
        {
            services.AddTransient(typeof(IMinimalApiEndpoint), type);
        }

        return services;
    }

    /// <summary>
    /// Maps all registered minimal API endpoints into the WebApplication, enabling their routes.
    /// </summary>
    /// <param name="app">The WebApplication to map the endpoints to.</param>
    /// <returns>The WebApplication for chaining.</returns>
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IMinimalApiEndpoint>();
        foreach (var endpoint in endpoints)
        {
            endpoint.HandleEndpoint(app);
        }

        return app;
    }
}