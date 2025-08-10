# 🛒 .NET E-Commerce Microservices Platform

A production-ready **microservices-based e-commerce application** built with **ASP.NET Core Web API**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Solution File Management (.slnx - Modern Approach)](#-solution-file-management-slnx---modern-approach)
- [Getting Started](#-getting-started)
- [Development Commands](#-development-commands)
- [Adding New Services](#-adding-new-services)
- [Deployment](#-deployment)
- [Best Practices](#-best-practices)
- [Resources](#-resources)

## 🎯 Overview

This solution demonstrates enterprise-grade microservices architecture with:

- **Independent Services**: Each business domain runs as a separate service
- **Clean Architecture**: Layered approach within each microservice
- **Shared Libraries**: Common functionality without tight coupling  
- **Scalable Design**: Database-per-service pattern
- **Testing Strategy**: Comprehensive unit and integration tests
- **Modern Solution Format**: Uses .slnx for better developer experience

### Core Services

| Service | Purpose | Technology |
|---------|---------|------------|
| **ProductApi** | Product catalog management | ASP.NET Core Web API |
| **OrderService** | Order processing & fulfillment | ASP.NET Core Web API |
| **AuthenticationService** | User authentication & JWT tokens | ASP.NET Core Web API |
| **ApiGateway** | Single entry point & request routing | ASP.NET Core / YARP |
| **SharedLibrary** | Common utilities & interfaces | Class Library |

## 🏗 Architecture

### Microservices Pattern
Each service operates independently with its own:
- Database instance
- Business logic
- API endpoints
- Deployment lifecycle

### Clean Architecture Layers

Each microservice follows a 4-layer clean architecture:

```
📁 ProductApi/
├── 🎯 ProductApi.Domain/        # Core business entities & rules
├── 🔧 ProductApi.Application/   # Use cases & business logic  
├── 🗄️ ProductApi.Infrastructure/ # Data access & external services
└── 🌐 ProductApi.Presentation/  # API controllers & HTTP layer
```

**Layer Dependencies:**
- **Domain**: No dependencies (pure business logic)
- **Application**: → Domain
- **Infrastructure**: → Application + Domain  
- **Presentation**: → Application + Infrastructure

### Shared Library Strategy

The `SharedLibrary` contains:
- JWT authentication configuration
- Base entities (`BaseEntity`, `AuditableEntity`)
- Exception handling middleware
- Common response models
- Cross-cutting concerns
- Shared constants & enums

> ⚠️ **Important**: Keep shared libraries minimal to prevent service coupling

## 📂 Project Structure

```
ecommerce-microservices/
├── 📁 src/
│   ├── 📦 SharedLibrary/
│   │   ├── Common/
│   │   ├── Exceptions/
│   │   ├── Middleware/
│   │   └── Models/
│   │
│   ├── 🛍️ ProductApi/
│   │   ├── ProductApi.Domain/
│   │   ├── ProductApi.Application/
│   │   ├── ProductApi.Infrastructure/
│   │   └── ProductApi.Presentation/
│   │
│   ├── 📦 OrderService/
│   │   ├── OrderService.Domain/
│   │   ├── OrderService.Application/
│   │   ├── OrderService.Infrastructure/
│   │   └── OrderService.Presentation/
│   │
│   ├── 🔐 AuthenticationService/
│   │   ├── AuthenticationService.Domain/
│   │   ├── AuthenticationService.Application/
│   │   ├── AuthenticationService.Infrastructure/
│   │   └── AuthenticationService.Presentation/
│   │
│   └── 🚪 ApiGateway/
│
├── 📁 tests/
│   ├── ProductApi.Tests/
│   ├── OrderService.Tests/
│   └── AuthenticationService.Tests/
│
├── 📁 docker/
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
│
├── 📄 Ecommerce.slnx
└── 📄 README.md
```

## 📁 Solution File Management (.slnx - Modern Approach)

### Why .slnx Format?

This project uses the **modern .slnx solution format** introduced in .NET 8 and Visual Studio 2022 17.9+. Here's why:

| Benefit | Description |
|---------|-------------|
| **🎯 Human Readable** | Clean JSON format that's easy to read and understand |
| **⚡ Performance** | ~50% smaller file size and faster parsing |
| **🔧 Better Merging** | Fewer merge conflicts in version control |
| **🚀 Future-Ready** | Modern format designed for current .NET ecosystem |
| **✨ Cleaner Structure** | No GUIDs, simplified project references |

### .slnx File Structure

```json
{
  "solution": {
    "path": "Ecommerce.slnx",
    "projects": [
      "src/SharedLibrary/SharedLibrary.csproj",
      "src/ProductApi/ProductApi.Domain/ProductApi.Domain.csproj",
      "src/ProductApi/ProductApi.Application/ProductApi.Application.csproj",
      "src/ProductApi/ProductApi.Infrastructure/ProductApi.Infrastructure.csproj",
      "src/ProductApi/ProductApi.Presentation/ProductApi.Presentation.csproj",
      "src/OrderService/OrderService.Domain/OrderService.Domain.csproj",
      "src/OrderService/OrderService.Application/OrderService.Application.csproj",
      "src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj",
      "src/OrderService/OrderService.Presentation/OrderService.Presentation.csproj",
      "src/AuthenticationService/AuthenticationService.Domain/AuthenticationService.Domain.csproj",
      "src/AuthenticationService/AuthenticationService.Application/AuthenticationService.Application.csproj",
      "src/AuthenticationService/AuthenticationService.Infrastructure/AuthenticationService.Infrastructure.csproj",
      "src/AuthenticationService/AuthenticationService.Presentation/AuthenticationService.Presentation.csproj",
      "src/ApiGateway/ApiGateway.csproj",
      "tests/ProductApi.Tests/ProductApi.Tests.csproj",
      "tests/OrderService.Tests/OrderService.Tests.csproj",
      "tests/AuthenticationService.Tests/AuthenticationService.Tests.csproj"
    ]
  }
}
```

### Creating the .slnx Solution

```bash
# Create modern solution file
dotnet new sln -n Ecommerce --use-program-main false

# Rename to .slnx format
mv Ecommerce.sln Ecommerce.slnx

# Verify creation
ls *.slnx
```

### Managing Projects in .slnx

```bash
# Add projects to solution
dotnet sln Ecommerce.slnx add src/**/*.csproj
dotnet sln Ecommerce.slnx add tests/**/*.csproj

# List all projects in solution
dotnet sln Ecommerce.slnx list

# Remove project from solution
dotnet sln Ecommerce.slnx remove <ProjectPath>

# Build entire solution
dotnet build Ecommerce.slnx

# Run all tests
dotnet test Ecommerce.slnx
```

### IDE Compatibility

| IDE/Editor | .slnx Support |
|------------|---------------|
| **Visual Studio 2022 17.9+** | ✅ Full Support |
| **VS Code** | ✅ Full Support |
| **JetBrains Rider** | ✅ Full Support |
| **Visual Studio 2019** | ❌ Not Supported |
| **Older IDEs** | ❌ Use .sln fallback |

### Legacy .sln Support

If you need to use the classic .sln format for compatibility reasons:

<details>
<summary>Click to expand legacy .sln instructions</summary>

#### Convert .slnx to .sln

```bash
# Method 1: Create new .sln and add all projects
dotnet new sln -n Ecommerce-classic
dotnet sln Ecommerce-classic.sln add src/**/*.csproj
dotnet sln Ecommerce-classic.sln add tests/**/*.csproj

# Method 2: Manual conversion
# Copy project list from .slnx and add to new .sln file
```

#### .sln File Structure (Reference)
```
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "ProductApi.Domain", "src\ProductApi\ProductApi.Domain\ProductApi.Domain.csproj", "{GUID}"
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
EndGlobal
```

</details>

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022 17.9+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server](https://www.microsoft.com/sql-server) or [PostgreSQL](https://www.postgresql.org/)

### Quick Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/ecommerce-microservices.git
   cd ecommerce-microservices
   ```

2. **Build the solution**
   ```bash
   dotnet build Ecommerce.slnx
   ```

3. **Run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

4. **Run individual service**
   ```bash
   cd src/ProductApi/ProductApi.Presentation
   dotnet run
   ```

### Manual Setup (Complete Solution)

```bash
# Create modern solution file
dotnet new sln -n Ecommerce
mv Ecommerce.sln Ecommerce.slnx

# Create shared library
dotnet new classlib -n SharedLibrary -o src/SharedLibrary

# Create ProductApi with Clean Architecture
dotnet new classlib -n ProductApi.Domain -o src/ProductApi/ProductApi.Domain
dotnet new classlib -n ProductApi.Application -o src/ProductApi/ProductApi.Application  
dotnet new classlib -n ProductApi.Infrastructure -o src/ProductApi/ProductApi.Infrastructure
dotnet new webapi -n ProductApi.Presentation -o src/ProductApi/ProductApi.Presentation

# Create OrderService
dotnet new classlib -n OrderService.Domain -o src/OrderService/OrderService.Domain
dotnet new classlib -n OrderService.Application -o src/OrderService/OrderService.Application
dotnet new classlib -n OrderService.Infrastructure -o src/OrderService/OrderService.Infrastructure  
dotnet new webapi -n OrderService.Presentation -o src/OrderService/OrderService.Presentation

# Create AuthenticationService
dotnet new classlib -n AuthenticationService.Domain -o src/AuthenticationService/AuthenticationService.Domain
dotnet new classlib -n AuthenticationService.Application -o src/AuthenticationService/AuthenticationService.Application
dotnet new classlib -n AuthenticationService.Infrastructure -o src/AuthenticationService/AuthenticationService.Infrastructure
dotnet new webapi -n AuthenticationService.Presentation -o src/AuthenticationService/AuthenticationService.Presentation

# Create API Gateway
dotnet new webapi -n ApiGateway -o src/ApiGateway

# Create test projects
dotnet new xunit -n ProductApi.Tests -o tests/ProductApi.Tests
dotnet new xunit -n OrderService.Tests -o tests/OrderService.Tests
dotnet new xunit -n AuthenticationService.Tests -o tests/AuthenticationService.Tests

# Add all projects to solution using glob patterns
dotnet sln Ecommerce.slnx add src/**/*.csproj
dotnet sln Ecommerce.slnx add tests/**/*.csproj
```

## 🛠 Development Commands

### Solution Management (.slnx)

```bash
# Create modern solution
dotnet new sln -n <SolutionName>
mv <SolutionName>.sln <SolutionName>.slnx

# Add projects using glob patterns
dotnet sln <Solution>.slnx add src/**/*.csproj
dotnet sln <Solution>.slnx add tests/**/*.csproj

# Add individual project
dotnet sln <Solution>.slnx add <ProjectPath>

# Remove project from solution
dotnet sln <Solution>.slnx remove <ProjectPath>

# List all projects in solution
dotnet sln <Solution>.slnx list

# Build entire solution
dotnet build <Solution>.slnx

# Run all tests in solution
dotnet test <Solution>.slnx

# Clean solution
dotnet clean <Solution>.slnx
```

### .NET Project Templates

| Command | Purpose | Example |
|---------|---------|---------|
| `dotnet new webapi -n <Name>` | Web API service | `dotnet new webapi -n ProductApi.Presentation` |
| `dotnet new classlib -n <Name>` | Class library | `dotnet new classlib -n ProductApi.Domain` |
| `dotnet new console -n <Name>` | Console application | `dotnet new console -n DataMigration` |
| `dotnet new worker -n <Name>` | Background service | `dotnet new worker -n EventProcessor` |
| `dotnet new xunit -n <Name>` | Unit test project | `dotnet new xunit -n ProductApi.Tests` |
| `dotnet new nunit -n <Name>` | NUnit test project | `dotnet new nunit -n OrderService.Tests` |
| `dotnet new grpc -n <Name>` | gRPC service | `dotnet new grpc -n ProductApi.Grpc` |

### Specialized Templates

| Command | Purpose |
|---------|---------|
| `dotnet new mvc -n <Name>` | MVC web application |
| `dotnet new webapp -n <Name>` | Razor Pages application |
| `dotnet new blazorserver -n <Name>` | Blazor Server application |
| `dotnet new blazorwasm -n <Name>` | Blazor WebAssembly application |

### Template Discovery

```bash
# List available templates
dotnet new list

# Search online templates
dotnet new search microservice

# Install template from NuGet
dotnet new install <PackageName>

# Uninstall template
dotnet new uninstall <PackageName>
```

## 🔧 Adding New Services

To add a new service (e.g., `InventoryService`):

### 1. Create Project Structure

```bash
# Create the 4-layer clean architecture
dotnet new classlib -n InventoryService.Domain -o src/InventoryService/InventoryService.Domain
dotnet new classlib -n InventoryService.Application -o src/InventoryService/InventoryService.Application
dotnet new classlib -n InventoryService.Infrastructure -o src/InventoryService/InventoryService.Infrastructure
dotnet new webapi -n InventoryService.Presentation -o src/InventoryService/InventoryService.Presentation

# Create test project
dotnet new xunit -n InventoryService.Tests -o tests/InventoryService.Tests
```

### 2. Add to Solution (.slnx)

```bash
# Add all new projects to solution
dotnet sln Ecommerce.slnx add src/InventoryService/**/*.csproj
dotnet sln Ecommerce.slnx add tests/InventoryService.Tests/InventoryService.Tests.csproj
```

### 3. Configure Project References

```bash
# Application layer references Domain
dotnet add src/InventoryService/InventoryService.Application reference src/InventoryService/InventoryService.Domain

# Infrastructure layer references Application and Domain
dotnet add src/InventoryService/InventoryService.Infrastructure reference src/InventoryService/InventoryService.Application
dotnet add src/InventoryService/InventoryService.Infrastructure reference src/InventoryService/InventoryService.Domain

# Presentation layer references Application and Infrastructure
dotnet add src/InventoryService/InventoryService.Presentation reference src/InventoryService/InventoryService.Application
dotnet add src/InventoryService/InventoryService.Presentation reference src/InventoryService/InventoryService.Infrastructure

# Add SharedLibrary reference where needed
dotnet add src/InventoryService/InventoryService.Application reference src/SharedLibrary
dotnet add src/InventoryService/InventoryService.Infrastructure reference src/SharedLibrary

# Test project references
dotnet add tests/InventoryService.Tests reference src/InventoryService/InventoryService.Domain
dotnet add tests/InventoryService.Tests reference src/InventoryService/InventoryService.Application
dotnet add tests/InventoryService.Tests reference src/InventoryService/InventoryService.Infrastructure
```

### 4. Complete Reference Setup Script

```bash
# ===== Syntax =====
# dotnet add <PROJECT_TO_ADD_REFERENCE_TO> reference <REFERENCE_PROJECT>

# ===== PRODUCTSERVICE REFERENCES =====
dotnet add src/ProductApi/ProductApi.Application reference src/ProductApi/ProductApi.Domain
dotnet add src/ProductApi/ProductApi.Infrastructure reference src/ProductApi/ProductApi.Application  
dotnet add src/ProductApi/ProductApi.Infrastructure reference src/ProductApi/ProductApi.Domain
dotnet add src/ProductApi/ProductApi.Presentation reference src/ProductApi/ProductApi.Application
dotnet add src/ProductApi/ProductApi.Presentation reference src/ProductApi/ProductApi.Infrastructure

# ===== ORDERSERVICE REFERENCES =====
dotnet add src/OrderService/OrderService.Application reference src/OrderService/OrderService.Domain
dotnet add src/OrderService/OrderService.Infrastructure reference src/OrderService/OrderService.Application
dotnet add src/OrderService/OrderService.Infrastructure reference src/OrderService/OrderService.Domain  
dotnet add src/OrderService/OrderService.Presentation reference src/OrderService/OrderService.Application
dotnet add src/OrderService/OrderService.Presentation reference src/OrderService/OrderService.Infrastructure

# ===== AUTHENTICATIONSERVICE REFERENCES =====
dotnet add src/AuthenticationService/AuthenticationService.Application reference src/AuthenticationService/AuthenticationService.Domain
dotnet add src/AuthenticationService/AuthenticationService.Infrastructure reference src/AuthenticationService/AuthenticationService.Application
dotnet add src/AuthenticationService/AuthenticationService.Infrastructure reference src/AuthenticationService/AuthenticationService.Domain
dotnet add src/AuthenticationService/AuthenticationService.Presentation reference src/AuthenticationService/AuthenticationService.Application  
dotnet add src/AuthenticationService/AuthenticationService.Presentation reference src/AuthenticationService/AuthenticationService.Infrastructure

# ===== SHARED LIBRARY REFERENCES =====
dotnet add src/ProductApi/ProductApi.Application reference src/SharedLibrary
dotnet add src/ProductApi/ProductApi.Infrastructure reference src/SharedLibrary
dotnet add src/OrderService/OrderService.Application reference src/SharedLibrary
dotnet add src/OrderService/OrderService.Infrastructure reference src/SharedLibrary
dotnet add src/AuthenticationService/AuthenticationService.Application reference src/SharedLibrary
dotnet add src/AuthenticationService/AuthenticationService.Infrastructure reference src/SharedLibrary

# ===== TEST PROJECT REFERENCES =====
dotnet add tests/ProductApi.Tests reference src/ProductApi/ProductApi.Domain
dotnet add tests/ProductApi.Tests reference src/ProductApi/ProductApi.Application
dotnet add tests/ProductApi.Tests reference src/ProductApi/ProductApi.Infrastructure
dotnet add tests/OrderService.Tests reference src/OrderService/OrderService.Domain
dotnet add tests/OrderService.Tests reference src/OrderService/OrderService.Application
dotnet add tests/OrderService.Tests reference src/OrderService/OrderService.Infrastructure
dotnet add tests/AuthenticationService.Tests reference src/AuthenticationService/AuthenticationService.Domain
dotnet add tests/AuthenticationService.Tests reference src/AuthenticationService/AuthenticationService.Application
dotnet add tests/AuthenticationService.Tests reference src/AuthenticationService/AuthenticationService.Infrastructure
```

### 5. Verify Solution Structure

```bash
# List all projects in solution
dotnet sln Ecommerce.slnx list

# Build entire solution
dotnet build Ecommerce.slnx

# Run all tests
dotnet test Ecommerce.slnx --verbosity normal
```

## 🐳 Deployment

### Docker Compose (Development)

```bash
# Build and start all services
docker-compose up --build

# Start in detached mode
docker-compose up -d

# View logs
docker-compose logs -f <service-name>

# Stop all services
docker-compose down

# Scale specific service
docker-compose up --scale productservice=3
```

### Individual Service Deployment

```bash
# Build Docker image
docker build -t productservice:latest -f src/ProductApi/ProductApi.Presentation/Dockerfile .

# Run container
docker run -p 8001:80 --name productservice productservice:latest

# Build all services
docker-compose build
```

### CI/CD with .slnx

```yaml
# GitHub Actions example
name: Build and Test
on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
    - name: Restore dependencies
      run: dotnet restore Ecommerce.slnx
    - name: Build
      run: dotnet build Ecommerce.slnx --no-restore
    - name: Test
      run: dotnet test Ecommerce.slnx --no-build --verbosity normal
```

## 💡 Best Practices

### Architecture Guidelines

- **Single Responsibility**: Each microservice handles one business domain
- **Database Per Service**: Avoid shared databases between services
- **API Contracts**: Use versioning and maintain backward compatibility
- **Event-Driven Communication**: Use message queues for service-to-service communication
- **Circuit Breaker Pattern**: Implement resilience patterns for external calls

### Code Organization

- **Domain Layer**: Keep pure business logic, no external dependencies
- **Application Layer**: Use CQRS with MediatR for complex business operations  
- **Infrastructure Layer**: Repository pattern with Entity Framework Core
- **Presentation Layer**: Thin controllers that delegate to application services

### Solution Management Best Practices

- **Use .slnx Format**: Leverage modern solution format for better developer experience
- **Glob Patterns**: Use `dotnet sln add src/**/*.csproj` for bulk operations
- **Consistent Naming**: Follow consistent naming conventions across all projects
- **Regular Maintenance**: Keep solution file clean and remove obsolete references
- **Team Agreement**: Ensure entire team uses compatible IDE versions (VS 2022 17.9+)

### Shared Library Strategy

✅ **Include in Shared Library:**
- Common DTOs and models
- Authentication/authorization logic
- Exception handling middleware
- Utility functions and extensions
- Cross-cutting concerns

❌ **Avoid in Shared Library:**
- Business-specific logic
- Domain entities
- Service implementations
- Heavy dependencies

### Testing Strategy

- **Unit Tests**: Test business logic in isolation
- **Integration Tests**: Test API endpoints and data access
- **Contract Tests**: Verify service contracts
- **End-to-End Tests**: Test complete user scenarios

### Development Workflow

```bash
# Daily workflow with .slnx
dotnet build Ecommerce.slnx    # Build all services
dotnet test Ecommerce.slnx     # Run all tests
dotnet run --project src/ApiGateway         # Start API Gateway
```

## 🔗 Technology Stack

| Layer | Technologies |
|-------|-------------|
| **Solution Format** | .slnx (Modern JSON-based) |
| **API Framework** | ASP.NET Core 9.0, Web API |
| **Data Access** | Entity Framework Core, SQL Server/PostgreSQL |
| **Authentication** | JWT Bearer Tokens, ASP.NET Core Identity |
| **API Gateway** | YARP (Yet Another Reverse Proxy) |
| **Message Queue** | RabbitMQ, Azure Service Bus |
| **Caching** | Redis, In-Memory Cache |
| **Logging** | Serilog, Application Insights |
| **Testing** | xUnit, Moq, FluentAssertions |
| **Containerization** | Docker, Docker Compose |

## 📚 Resources

### Documentation
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft .NET Microservices Guide](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/)
- [Domain-Driven Design Reference](https://domainlanguage.com/ddd/reference/)
- [.slnx Format Documentation](https://docs.microsoft.com/en-us/visualstudio/ide/solutions-and-projects-in-visual-studio)

### Tools & Libraries
- [MediatR](https://github.com/jbogard/MediatR) - CQRS and Mediator pattern
- [AutoMapper](https://automapper.org/) - Object-to-object mapping
- [FluentValidation](https://fluentvalidation.net/) - Input validation
- [Polly](https://github.com/App-vNext/Polly) - Resilience patterns

## ✅ Benefits of This Architecture

| Benefit | Description |
|---------|-------------|
| **Modern Solution Format** | .slnx provides better developer experience and version control |
| **Scalability** | Scale services independently based on demand |
| **Maintainability** | Clear separation of concerns and dependencies |
| **Testability** | Easy unit testing with dependency injection |
| **Flexibility** | Technology diversity across services |
| **Reliability** | Fault isolation prevents cascade failures |
| **Team Autonomy** | Teams can work independently on services |
| **Developer Experience** | Faster builds and cleaner solution files |

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

**Requirements for Contributors:**
- Visual Studio 2022 17.9+ or compatible IDE
- .NET 9.0 SDK
- Familiarity with .slnx solution format

**Author:** haris18896  
**Version:** 2.0.0  
**Last Updated:** August 2025