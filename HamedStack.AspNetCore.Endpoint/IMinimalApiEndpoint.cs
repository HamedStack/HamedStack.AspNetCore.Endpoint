using Microsoft.AspNetCore.Routing;

namespace HamedStack.AspNetCore.Endpoint;

/// <summary>
/// Defines a contract for minimal API endpoint handlers. Implementations should define how an endpoint is handled.
/// </summary>
public interface IMinimalApiEndpoint
{
    /// <summary>
    /// Handles the endpoint registration with the specified endpoint route builder.
    /// </summary>
    /// <param name="endpoint">The endpoint route builder to configure.</param>
    void HandleEndpoint(IEndpointRouteBuilder endpoint);
}