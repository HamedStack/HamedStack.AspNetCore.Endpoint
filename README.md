# HamedStack.AspNetCore.Endpoint

This documentation provides detailed instructions for handling minimal API endpoints in an ASP.NET Core application using the latest version of ASP.NET Core. It introduces the `IMinimalApiEndpoint` interface for defining endpoint handlers and the `MinimalApiEndpointsExtensions` class for registering and mapping these handlers.

## IMinimalApiEndpoint Interface

### Summary
Defines a contract for minimal API endpoint handlers. Implementations of this interface should define the logic for handling an API endpoint.

### Members

#### HandleEndpoint
```csharp
void HandleEndpoint(IEndpointRouteBuilder endpoint);
```
##### Parameters
- `endpoint`: The `IEndpointRouteBuilder` instance used to configure the endpoint.

##### Description
Handles the registration of the endpoint with the specified endpoint route builder. Implementing classes should provide the specific logic for configuring the endpoint.

## MinimalApiEndpointsExtensions Class

### Summary
Provides extension methods to register and map minimal API endpoints within an ASP.NET Core application.

### Methods

#### AddMinimalApiEndpoints
```csharp
public static IServiceCollection AddMinimalApiEndpoints(this IServiceCollection services)
```
##### Parameters
- `services`: The `IServiceCollection` to add the services to.

##### Returns
`IServiceCollection`: The service collection for chaining.

##### Description
Registers all implementations of `IMinimalApiEndpoint` found in the application into the service collection. This method scans the application for types that implement the `IMinimalApiEndpoint` interface and registers them as transient services.

##### Example Usage
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMinimalApiEndpoints();
```

#### MapMinimalApiEndpoints
```csharp
public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
```
##### Parameters
- `app`: The `WebApplication` to map the endpoints to.

##### Returns
`WebApplication`: The web application for chaining.

##### Description
Maps all registered minimal API endpoints into the `WebApplication`, enabling their routes. This method retrieves all services that implement the `IMinimalApiEndpoint` interface and invokes their `HandleEndpoint` method to configure the endpoints.

##### Example Usage
```csharp
var app = builder.Build();
app.MapMinimalApiEndpoints();
app.Run();
```

## Implementation Example

Below is a complete example demonstrating how to implement and use the minimal API endpoint handler system in an ASP.NET Core application.

### Step 1: Define an Endpoint Handler
Create a class that implements the `IMinimalApiEndpoint` interface.

```csharp
public class HelloWorldEndpoint : IMinimalApiEndpoint
{
    public void HandleEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/hello", () => "Hello, World!");
    }
}
```

### Step 2: Register the Endpoint Handlers
In your program setup, register the endpoint handlers using the provided extension method.

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMinimalApiEndpoints();
```

### Step 3: Configure the Application
Ensure your application maps the registered endpoints and runs correctly.

```csharp
var app = builder.Build();
app.MapMinimalApiEndpoints();
app.Run();
```

### Full Example
Combining all steps into a complete example:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public interface IMinimalApiEndpoint
{
    void HandleEndpoint(IEndpointRouteBuilder endpoint);
}

public class HelloWorldEndpoint : IMinimalApiEndpoint
{
    public void HandleEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/hello", () => "Hello, World!");
    }
}

public static class MinimalApiEndpointsExtensions
{
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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMinimalApiEndpoints();

var app = builder.Build();
app.MapMinimalApiEndpoints();
app.Run();
```

## Conclusion

By following the outlined steps and utilizing the provided interface and extension methods, you can create a scalable and maintainable system for managing minimal API endpoints in your ASP.NET Core application. This approach allows for a clean separation of endpoint configuration logic and centralizes the registration and mapping of endpoints.
