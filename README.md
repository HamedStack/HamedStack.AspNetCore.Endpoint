## **Overview**

The `MinimalApiEndpointBase` library simplifies the organization, registration, and mapping of minimal API endpoints in your ASP.NET Core applications. It uses an abstract base class for endpoint logic, making your API endpoints modular and testable.

---

## **Key Components**

### **1. `MinimalApiEndpointBase`**
This is the abstract base class that your API endpoints should inherit from. It provides:

- **Properties**:
  - `Application`: The `WebApplication` instance for the current application.
  - `Services`: An `IServiceProvider` instance for dependency injection.

- **Abstract Method**:
  - `HandleEndpoint(IEndpointRouteBuilder endpoint)`: Must be implemented by derived classes to define endpoint routing and logic.

- **Method**:
  - `Initialize(WebApplication app)`: Used internally to associate the base class with the current `WebApplication`.

---

### **2. Extension Methods**

#### **`AddMinimalApiEndpoints(IServiceCollection services)`**
- Scans all loaded assemblies to find implementations of `MinimalApiEndpointBase`.
- Registers these implementations as services in the DI container.

#### **`MapMinimalApiEndpoints(WebApplication app)`**
- Resolves all registered `MinimalApiEndpointBase` services.
- Calls their `Initialize` and `HandleEndpoint` methods to configure routes within the application.

---

## **Setup and Usage**

Follow these steps to integrate and use the library in your ASP.NET Core application:

### **1. Create an Endpoint Class**
Define an endpoint by inheriting from `MinimalApiEndpointBase`:

```csharp
public class WeatherEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/weather", async context =>
        {
            var forecast = new[] { "Sunny", "Cloudy", "Rainy" };
            await context.Response.WriteAsJsonAsync(forecast);
        });
    }
}
```

### **2. Register Endpoints in Dependency Injection**
In the `Program.cs` file, register the endpoints by calling `AddMinimalApiEndpoints`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMinimalApiEndpoints();

var app = builder.Build();
```

### **3. Map Endpoints to the WebApplication**
Use `MapMinimalApiEndpoints` to map all registered endpoints during the application configuration:

```csharp
// Configure the HTTP request pipeline.
app.MapMinimalApiEndpoints();

app.Run();
```

---

## **Example Application**

Here’s a complete example of an application using the library:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

public class WeatherEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/weather", async context =>
        {
            var forecast = new[] { "Sunny", "Cloudy", "Rainy" };
            await context.Response.WriteAsJsonAsync(forecast);
        });
    }
}

var builder = WebApplication.CreateBuilder(args);

// Register all endpoint implementations.
builder.Services.AddMinimalApiEndpoints();

var app = builder.Build();

// Map endpoints to routes.
app.MapMinimalApiEndpoints();

app.Run();
```

---

## **Advanced Features**

### **Dynamic Dependency Injection**
Use the `Services` property in your endpoint implementation to resolve dependencies:

```csharp
public class GreetingEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/greet", async context =>
        {
            var logger = Services.GetRequiredService<ILogger<GreetingEndpoint>>();
            logger.LogInformation("Greet endpoint called");

            await context.Response.WriteAsync("Hello, world!");
        });
    }
}
```

### Access to `Application`
The `Application` property provides access to the `WebApplication` instance, enabling you to perform application-level configurations or access global middleware and services:

```csharp
public class CustomMiddlewareEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoints)
    {
        Application.Use(async (context, next) =>
        {
            // Custom logic before passing to the next middleware
            context.Response.Headers.Add("X-Custom-Header", "HelloFromMiddleware");
            await next();
        });

        endpoints.MapGet("/custom-middleware", async context =>
        {
            await context.Response.WriteAsync("Middleware is applied!");
        });
    }
}
```

### Read form `Configuration`
You can use the `Configuration` property to access application settings, such as those defined in `appsettings.json`, environment variables, or other sources.

```csharp
public class GreetingEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/greet", async context =>
        {
            var greetingMessage = Configuration["Greeting"]; // Access configuration setting
            await context.Response.WriteAsync(greetingMessage);
        });
    }
}
```



