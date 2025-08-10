# SharedLibrary - E-commerce Microservices Common Components

## 📋 Table of Contents
- [Introduction](#introduction)
- [What This Library Does](#what-this-library-does)
- [Installation & Setup](#installation--setup)
- [Project Structure](#project-structure)
- [Features Deep Dive](#features-deep-dive)
- [Usage Examples](#usage-examples)
- [Best Practices & Improvements](#best-practices--improvements)
- [Common Pitfalls to Avoid](#common-pitfalls-to-avoid)
- [Conclusion](#conclusion)

## 🚀 Introduction

Welcome to **SharedLibrary** - a reusable .NET library designed for e-commerce microservices architecture! 

As a beginner, think of this library as a **toolbox** that contains common functionality that multiple microservices in your e-commerce application will need. Instead of writing the same code over and over in each service (authentication, logging, error handling), you create it once here and reuse it everywhere.

### Why Use a Shared Library?
- **DRY Principle**: Don't Repeat Yourself - write common code once
- **Consistency**: All your microservices behave the same way
- **Maintainability**: Fix a bug once, and it's fixed everywhere
- **Security**: Centralized security implementations reduce vulnerabilities

## 🎯 What This Library Does

This shared library provides **4 main features**:

1. **🔐 JWT Authentication**: Handles user authentication across all your microservices
2. **🛡️ Security Middleware**: Protects your services from unauthorized access
3. **📝 Structured Logging**: Logs errors and information consistently
4. **🗄️ Database Integration**: Simplifies Entity Framework Core setup

## 📦 Installation & Setup

### Step 1: Install Required Packages

In your SharedLibrary project folder (`src/SharedLibrarySolution`), run:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Serilog.AspNetCore
```

### Step 2: Build the Project

```bash
dotnet build
```

### Step 3: Reference in Other Projects

In your microservice projects, add a reference to this shared library:

```bash
dotnet add reference ../SharedLibrarySolution/SharedLibrarySolution.csproj
```

## 📁 Project Structure

```
SharedLibrarySolution/
├── Responses/
│   └── Response.cs                 # Standard API response format
├── Middleware/
│   ├── ListenToOnlyApiGateway.cs  # API Gateway security
│   └── GlobalException.cs         # Global error handling
├── Logger/
│   └── LogException.cs            # Logging utilities
├── Interface/
│   └── IGenericInterface.cs       # Generic repository pattern
└── DependencyInjection/
    ├── SharedServiceContainer.cs  # Main service registration
    └── JWTAuthenticationScheme.cs # JWT configuration
```

## 🔍 Features Deep Dive

### 1. 📤 Response Model (`Response.cs`)

**What it does**: Provides a consistent format for all API responses across your microservices.

```csharp
public record Response(bool Flag = false, string Message = null!)
```

**Why use it**: 
- **Consistency**: All your APIs return the same response structure
- **Client-friendly**: Frontend developers know exactly what to expect
- **Error handling**: Easy to check if an operation succeeded or failed

**Example Usage**:
```csharp
// Success response
return new Response(true, "User created successfully");

// Error response  
return new Response(false, "Email already exists");
```

### 2. 🛡️ Security Middleware

#### A. API Gateway Middleware (`ListenToOnlyApiGateway.cs`)

**What it does**: Ensures that your microservices can ONLY be accessed through your API Gateway, not directly.

**How it works**:
1. Checks for a special header called `Api-Gateway`
2. If the header is missing, returns a 503 error
3. If present, allows the request to continue

**Why use it**:
- **Security**: Prevents direct access to your microservices
- **Control**: All requests must go through your gateway for routing/auth
- **Monitoring**: Centralized logging and rate limiting

**Real-world scenario**: 
Imagine your User Service runs on `localhost:5001`. Without this middleware, hackers could directly call your service. With it, they MUST go through your API Gateway first.

#### B. Global Exception Handler (`GlobalException.cs`)

**What it does**: Catches ALL unhandled exceptions in your application and converts them to user-friendly responses.

**How it works**:
1. Wraps your entire request pipeline
2. Catches different types of exceptions (timeouts, file not found, etc.)
3. Returns appropriate HTTP status codes and messages
4. Logs the original exception for debugging

**Exception Types Handled**:
- **429 Too Many Requests**: "Slow down, you're making too many requests!"
- **401 Unauthorized**: "You need to log in first"
- **403 Forbidden**: "You don't have permission for this"
- **408 Request Timeout**: "Request took too long"
- **404 File Not Found**: "The file you're looking for doesn't exist"
- **500 Internal Server Error**: Generic fallback for all other errors

**Why use it**:
- **User Experience**: Users see friendly messages instead of technical errors
- **Security**: Hides internal implementation details from potential attackers
- **Debugging**: Logs detailed error info for developers

### 3. 📝 Logging (`LogException.cs`)

**What it does**: Provides a simple way to log exceptions to multiple destinations.

**Logging Destinations**:
- **File**: Permanent record for analysis (`Log.Information`)
- **Console**: Immediate visibility during development (`Log.Warning`)
- **Debugger**: IDE integration for debugging (`Log.Debug`)

**Example Usage**:
```csharp
try 
{
    // Your risky code here
    var user = await userService.GetUserAsync(id);
}
catch (Exception ex)
{
    LogException.LogExceptions(ex);
    throw; // Re-throw to let global handler manage the response
}
```

### 4. 🗄️ Generic Repository Interface (`IGenericInterface.cs`)

**What it does**: Defines a standard contract for database operations that any entity can use.

**Operations Provided**:
- `CreateAsync`: Add new records
- `UpdateAsync`: Modify existing records  
- `DeleteAsync`: Remove records
- `GetAllAsync`: Fetch all records
- `FindByIdAsync`: Find by primary key
- `GetByAsync`: Find by custom conditions

**Why use it**:
- **Consistency**: All your entities have the same CRUD operations
- **Testability**: Easy to mock for unit testing
- **DRY**: Write database logic once, use everywhere

**Example Implementation**:
```csharp
public class UserRepository : IGenericInterface<User>
{
    public async Task<Response> CreateAsync(User user)
    {
        // Implementation here
        return new Response(true, "User created");
    }
    
    // ... other methods
}
```

### 5. 🔗 Dependency Injection

#### A. Main Service Container (`SharedServiceContainer.cs`)

**What it does**: Configures all the shared services your application needs.

**Services Configured**:
- **Database Context**: Entity Framework Core with SQL Server
- **Logging**: Serilog with file, console, and debug output
- **JWT Authentication**: Token-based security

**Key Configuration Details**:

```csharp
// Database with retry logic
services.AddDbContext<TContext>(option => option.UseSqlServer(
    configuration.GetConnectionString("DefaultConnection"), 
    sqlserverOption => sqlserverOption.EnableRetryOnFailure()
));

// Serilog logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(path: $"{fileName}-.text")  // Daily rotating files
    .WriteTo.Console()                        // Console output
    .WriteTo.Debug()                          // Visual Studio debug window
    .CreateLogger();
```

#### B. JWT Authentication (`JWTAuthenticationScheme.cs`)

**What it does**: Configures JSON Web Token authentication for your microservices.

**Configuration Required** (in `appsettings.json`):
```json
{
  "authentication": {
    "key": "your-super-secret-key-at-least-32-characters",
    "issuer": "your-app-name",
    "audience": "your-app-users"
  }
}
```

**How JWT Works** (Simplified):
1. User logs in with username/password
2. Server creates a signed token
3. User sends token with each request
4. Server validates token signature
5. If valid, request is processed

## 💻 Usage Examples

### Example 1: Setting Up in a Microservice

**Program.cs** in your User Service:
```csharp
using Ecommerce.SharedLibrary.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add your DbContext and shared services
builder.Services.AddSharedServices<UserDbContext>(
    builder.Configuration, 
    "UserServiceLogs"  // Log file prefix
);

// Add your specific services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Use shared middleware (ORDER MATTERS!)
app.UseSharedPlicies();  // This adds GlobalException + ApiGateway middleware

// Your specific middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

### Example 2: Using in a Controller

**UserController.cs**:
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // JWT authentication required
public class UserController : ControllerBase
{
    private readonly IGenericInterface<User> _userRepository;
    
    public UserController(IGenericInterface<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<Response>> CreateUser(User user)
    {
        try
        {
            var result = await _userRepository.CreateAsync(user);
            
            if (result.Flag)
                return Ok(result);  // 200 OK
            else
                return BadRequest(result);  // 400 Bad Request
        }
        catch (Exception ex)
        {
            // GlobalException middleware will handle this
            LogException.LogExceptions(ex);
            throw;
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }
}
```

### Example 3: API Gateway Configuration

Your API Gateway needs to add the special header:

```csharp
// In your API Gateway
context.Request.Headers.Add("Api-Gateway", "internal-service-call");
```

### Example 4: Connection String Setup

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EcommerceUserDB;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "authentication": {
    "key": "this-is-my-super-secret-key-for-jwt-tokens-32-chars-minimum",
    "issuer": "EcommerceApp",
    "audience": "EcommerceUsers"
  }
}
```

## ⚡ Best Practices & Improvements

### 🔐 Security Improvements

#### Current Issues:
1. **Hard-coded header name**: `"Api-Gateway"` is predictable
2. **No header value validation**: Just checks if header exists
3. **JWT lifetime validation disabled**: `ValidateLifetime = false`

#### Recommended Fixes:

**1. Secure API Gateway Middleware:**
```csharp
public class ListenToOnlyApiGateway(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var expectedHeader = configuration["ApiGateway:HeaderName"] ?? "X-Internal-Gateway";
        var expectedValue = configuration["ApiGateway:SecretValue"];
        
        var signedHeader = context.Request.Headers[expectedHeader].FirstOrDefault();
        
        // Check both existence AND value
        if (signedHeader != expectedValue)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Service Unavailable");
            return;
        }

        await next(context);
    }
}
```

**Configuration:**
```json
{
  "ApiGateway": {
    "HeaderName": "X-Internal-Gateway-Auth",
    "SecretValue": "your-super-secret-internal-key-here"
  }
}
```

**2. Enable JWT Lifetime Validation:**
```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateLifetime = true,  // Enable this!
    ClockSkew = TimeSpan.FromMinutes(5)  // Allow 5 min clock drift
    // ... other settings
};
```

### 🏗️ Code Structure Improvements

#### 1. Add Configuration Options Pattern

**Create JwtOptions.cs:**
```csharp
public class JwtOptions
{
    public const string SectionName = "Authentication";
    
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}
```

**Updated JWT Configuration:**
```csharp
public static IServiceCollection AddJwtAuthenticationScheme(
    this IServiceCollection services, 
    IConfiguration configuration)
{
    var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
    
    if (string.IsNullOrEmpty(jwtOptions?.Key))
        throw new ArgumentException("JWT Key is required");
        
    services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
    
    // Rest of JWT configuration...
}
```

#### 2. Improve Generic Interface

**Add Pagination Support:**
```csharp
public interface IGenericInterface<T> where T : class
{
    // Existing methods...
    
    // New improved methods
    Task<PagedResult<T>> GetPagedAsync(int page, int pageSize);
    Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate, int limit = 100);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage => Page * PageSize < TotalCount;
}
```

#### 3. Enhanced Response Model

```csharp
public record Response<T>(bool Flag = false, string Message = null!, T? Data = default)
{
    // Static factory methods for common scenarios
    public static Response<T> Success(T data, string message = "Operation successful") 
        => new(true, message, data);
        
    public static Response<T> Failure(string message) 
        => new(false, message);
}

// Keep the original for backward compatibility
public record Response(bool Flag = false, string Message = null!) : Response<object>(Flag, Message);
```

### 🚨 Error Handling Improvements

#### 1. Custom Exception Types

```csharp
// Create specific exception types
public class BusinessLogicException : Exception
{
    public BusinessLogicException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(Dictionary<string, string[]> errors) 
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }
}
```

#### 2. Enhanced Global Exception Handler

```csharp
// In GlobalException.cs, add handling for custom exceptions
if (ex is BusinessLogicException)
{
    title = "Business Logic Error";
    message = ex.Message;
    statusCode = StatusCodes.Status400BadRequest;
    await ModifyHeader(context, title, message, statusCode);
}
else if (ex is ValidationException validationEx)
{
    await context.Response.WriteAsync(JsonSerializer.Serialize(new ValidationProblemDetails(validationEx.Errors)
    {
        Status = StatusCodes.Status400BadRequest
    }));
    return;
}
```

### 📊 Logging Improvements

#### 1. Structured Logging

```csharp
public static class LogException
{
    public static void LogExceptions(Exception ex, string? userId = null, string? correlationId = null)
    {
        // Structured logging with context
        Log.Error(ex, "Exception occurred for User: {UserId}, CorrelationId: {CorrelationId}", 
            userId, correlationId);
    }
    
    public static void LogBusinessEvent(string eventName, object eventData)
    {
        Log.Information("Business Event: {EventName} with data {@EventData}", eventName, eventData);
    }
}
```

### 🔧 Configuration Improvements

#### 1. Environment-Specific Settings

**appsettings.Development.json:**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning"
      }
    }
  }
}
```

**appsettings.Production.json:**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning"
    }
  }
}
```

## ⚠️ Common Pitfalls to Avoid

### 1. **Middleware Order Matters!**
```csharp
// ❌ WRONG ORDER
app.UseAuthentication();
app.UseSharedPolicies();  // Global exception should be first!

// ✅ CORRECT ORDER  
app.UseSharedPolicies();   // Exception handling first
app.UseAuthentication();   // Then authentication
app.UseAuthorization();    // Then authorization
```

### 2. **Don't Forget Error Handling**
```csharp
// ❌ BAD: No error handling
public async Task<Response> CreateUser(User user)
{
    return await _userRepository.CreateAsync(user);
}

// ✅ GOOD: Proper error handling
public async Task<Response> CreateUser(User user)
{
    try
    {
        // Validate input
        if (string.IsNullOrEmpty(user.Email))
            return new Response(false, "Email is required");
            
        return await _userRepository.CreateAsync(user);
    }
    catch (Exception ex)
    {
        LogException.LogExceptions(ex);
        throw; // Let global handler manage the response
    }
}
```

### 3. **JWT Secret Key Security**
```csharp
// ❌ NEVER do this
var key = "mysecretkey";

// ✅ DO this - minimum 32 characters, use environment variables in production
var key = configuration["Authentication:Key"]; // From appsettings or environment
```

### 4. **Database Connection Resilience**
```csharp
// ✅ GOOD: Your code already includes retry logic
services.AddDbContext<TContext>(option => option.UseSqlServer(
    configuration.GetConnectionString("DefaultConnection"), 
    sqlserverOption => sqlserverOption.EnableRetryOnFailure()
));
```

## 🎯 Conclusion

Congratulations! You've built a solid foundation for microservices architecture. This shared library provides:

- ✅ **Consistent authentication** across all services
- ✅ **Centralized error handling** for better user experience  
- ✅ **Security middleware** to protect your services
- ✅ **Structured logging** for easier debugging
- ✅ **Generic database patterns** for faster development

### Next Steps for Learning:

1. **Practice**: Create 2-3 microservices using this shared library
2. **Testing**: Learn to write unit tests for your repositories and services
3. **Monitoring**: Add health checks and metrics
4. **Documentation**: Use tools like Swagger/OpenAPI for API documentation
5. **Advanced Topics**: Learn about message queues, caching, and service discovery

### Useful Resources:

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Serilog Documentation](https://serilog.net/)
- [JWT.io](https://jwt.io/) - Learn more about JSON Web Tokens

Remember: **Start small, build incrementally, and always prioritize security and maintainability!** 🚀

---

*Happy coding! If you have questions about any part of this library, feel free to ask. Learning .NET is a journey, and you're on the right path!* 😊