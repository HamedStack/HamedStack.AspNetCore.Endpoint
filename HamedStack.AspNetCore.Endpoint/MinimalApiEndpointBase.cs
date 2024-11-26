using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace HamedStack.AspNetCore.Endpoint;

/// <summary>
/// Serves as a base class for minimal API endpoints in an ASP.NET Core application.
/// </summary>
public abstract class MinimalApiEndpointBase
{
    /// <summary>
    /// Gets the <see cref="WebApplication"/> instance for the current application.
    /// This property provides access to the core web application for configuration and service resolution.
    /// </summary>
    protected WebApplication Application { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance used for resolving application services.
    /// This allows derived classes to resolve dependencies from the application's service container.
    /// </summary>
    protected IServiceProvider Services => Application.Services;

    /// <summary>
    /// Gets the <see cref="IConfiguration"/> instance for accessing application configuration settings.
    /// This property provides access to configuration values stored in appsettings or environment variables.
    /// </summary>
    protected IConfiguration Configuration => Application.Configuration;

    /// <summary>
    /// Configures and handles the minimal API endpoint using the provided <see cref="IEndpointRouteBuilder"/>.
    /// Derived classes must override this method to define their specific routing and handling logic.
    /// </summary>
    /// <param name="endpoint">The <see cref="IEndpointRouteBuilder"/> used to configure API routes.</param>
    public abstract void HandleEndpoint(IEndpointRouteBuilder endpoint);

    /// <summary>
    /// Initializes the base class with the provided <see cref="WebApplication"/> instance.
    /// This method must be called before configuring the endpoint to ensure the application context is available.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance representing the current application.</param>
    internal void Initialize(WebApplication app)
    {
        Application = app;
    }
}