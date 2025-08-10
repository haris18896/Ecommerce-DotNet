# 🛒 .NET Ecommerce Microservices — Setup & Command Reference

This project follows a **.NET Microservices architecture** for building an Ecommerce platform.

---

## 📂 Services & Components
The solution will include:

1. **SharedLibrarySolution** — Common reusable code (DTOs, exceptions, helpers, etc.).
2. **ProductService** — Manages product catalog.
3. **OrderService** — Handles orders and processing.
4. **AuthenticationService** — User registration, login, JWT authentication.
5. **API Gateway** — Single entry point for client requests.
6. **Unit Testing** — Test projects for all services.

---

## 🛠 .NET Project Creation Commands

### **1️⃣ Create a Web API Service**
For microservices like ProductService, OrderService, AuthenticationService, API Gateway:

```sh
dotnet new webapi -n <ServiceName>
# Example
dotnet new webapi -n ProductService
```

---

### **2️⃣ Create a Class Library**

For reusable shared code:

```sh
dotnet new classlib -n <LibraryName>
#Example
dotnet new classlib -n SharedLibrarySolution
```

---

### **3️⃣ Create a Console App**

For background jobs, quick tests, or CLI tools:

```sh
dotnet new console -n <AppName>
```

---

### **4️⃣ Create a Worker Service**

For background services, event consumers, queue processing:

```sh
dotnet new worker -n <WorkerName>
```

---

### **5️⃣ Create a Unit Test Project (xUnit)**

For testing each service:

```sh
dotnet new xunit -n <TestProjectName>
#Example
dotnet new xunit -n ProductService.Tests
```

---

### **6️⃣ Create a Unit Test Project (NUnit)**

Alternative testing framework:

```sh
dotnet new nunit -n <TestProjectName>
```

---

### **7️⃣ Create a gRPC Service**

For high-performance inter-service communication:

```sh
dotnet new grpc -n <ServiceName>
```

---

### **8️⃣ Create an MVC Web App**

For server-rendered pages (if needed for admin panel):

```sh
dotnet new mvc -n <AppName>
```

---

### **9️⃣ Create a Razor Pages Web App**

Lightweight page-based web app:

```sh
dotnet new webapp -n <AppName>
```

---

### **🔟 Create a Blazor Server App**

For interactive UIs using C# server-side rendering:

```sh
dotnet new blazorserver -n <AppName>
```

---

### **1️⃣1️⃣ Create a Blazor WebAssembly App**

For running C# directly in the browser:

```sh
dotnet new blazorwasm -n <AppName>
```

---

### **1️⃣2️⃣ Create a Solution File**

To group multiple projects:

```sh
dotnet new sln -n <SolutionName>
```

---

### **1️⃣3️⃣ Add a Project to a Solution**

```sh
dotnet sln <SolutionName>.sln add <ProjectPath>
```

**Example:**

```sh
dotnet sln Ecommerce.sln add ./src/ProductService/ProductService.csproj
```

---

### **1️⃣4️⃣ Run All Available Templates**

```sh
dotnet new list
```

---

### **1️⃣5️⃣ Search Online Templates**

```sh
dotnet new search <keyword>
#Example:
dotnet new search microservice
```

---

### **1️⃣6️⃣ Install a New Template from NuGet**

```sh
dotnet new install <PackageName>
```

---

## 📋 Suggested Project Structure

```css
ecommerce-root/
├─ src/
│  ├─ SharedLibrarySolution/
│  ├─ ProductService/
│  ├─ OrderService/
│  ├─ AuthenticationService/
│  ├─ ApiGateway/
├─ tests/
│  ├─ ProductService.Tests/
│  ├─ OrderService.Tests/
│  ├─ AuthenticationService.Tests/
└─ README.md
```

---

## 🚀 Quick Start — Creating the Whole Solution

```sh
# Create solution
dotnet new sln -n Ecommerce

# Create projects
dotnet new classlib -n SharedLibrarySolution -o src/SharedLibrarySolution
dotnet new webapi -n ProductService -o src/ProductService
dotnet new webapi -n OrderService -o src/OrderService
dotnet new webapi -n AuthenticationService -o src/AuthenticationService
dotnet new webapi -n ApiGateway -o src/ApiGateway

# Create test projects
dotnet new xunit -n ProductService.Tests -o tests/ProductService.Tests
dotnet new xunit -n OrderService.Tests -o tests/OrderService.Tests
dotnet new xunit -n AuthenticationService.Tests -o tests/AuthenticationService.Tests

# Add projects to solution
dotnet sln Ecommerce.sln add src/**/**/*.csproj
dotnet sln Ecommerce.sln add tests/**/**/*.csproj
```

---

## 💡 Notes

* Keep **SharedLibrarySolution** minimal to avoid tight coupling.
* Use **database-per-service** pattern for microservices.
* Consider using **Docker** and `docker-compose` for local dev with RabbitMQ and databases.
* API Gateway can be implemented with **YARP** or **Ocelot**.

```

---

This preserves the **exact "sh / css" code block style** you wanted while keeping it clean and copy-paste friendly.  

If you want, I can now **add a visual architecture diagram** to this README so it’s also a quick onboarding reference for new devs. That would make this document a full **developer handbook**.  

Do you want me to add that diagram next?
```
