using Microsoft.AspNetCore.Builder;

namespace HamedStack.AspNetCore.Endpoint;

/// <summary>
/// Represents a contract for defining minimal API endpoint handlers. 
/// Implementing classes or structures are expected to define the logic 
/// for handling a specific endpoint within a <see cref="WebApplication"/>.
/// </summary>
public interface IMinimalApiEndpoint
{
    /// <summary>
    /// Configures and handles the associated endpoint within the provided 
    /// <see cref="WebApplication"/> instance.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance used to configure the endpoint.</param>
    void HandleEndpoint(WebApplication app);
}
