# `PRODUCT SERVICE`

1. Create Application => classlib
2. Create Domain => classlib
3. Create Infrustructure => classlib
4. Create Presentation=> web api
5. install required packages in the Presentation
6. add `infrustrcutre` reference to the `presentation`
7. add `applciation` reference to the `infructructure`
8. add `domain & SharedLibrarySolutioin` refernce to `application`
9. build presentation to test

* We need to install docker for mac, and then create sqlServer there, which we can then connect with any dbAdmi like postgresql, bdeaver, or MySQL work bench
* the database migration and update depends on which database for which app to be migrated and updated
```sh
# Start SQL Container in Docker
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=<System_Admininstrator_password>" \
  -p 1435:1433 \
  --platform linux/amd64 \
  --name <SQL_SERVER_Container> \
  -d mcr.microsoft.com/mssql/server:2022-latest

# Connect SQL server inside container
docker exec -it <SQL_SERVER_Container> /opt/mssql-tools18/bin/sqlcmd \
   -S localhost -U SA -P '<System_Admininstrator_password>' -No

# Create Database and Login
SELECT name FROM sys.databases;
GO
CREATE DATABASE EcommerceDB;
GO
SELECT name FROM sys.sql_logins;
GO
CREATE LOGIN xhadow WITH PASSWORD = 'Incorrect@123';
GO
USE EcommerceDB;
CREATE USER xhadow FOR LOGIN xhadow;
ALTER ROLE db_owner ADD MEMBER xhadow;
GO

# Then Connect with the server
# Server: localhost,1435
# Database: EcommerceDB
# User: xhadow
# Password: Incorrect@123
# Authentication: SQL Server Authentication
  

# Install Entity Framework tools
dotnet tool install --global dotnet-ef

# Create and apply migrations
dotnet ef migrations add Initial \
  --project src/ProductApi/ProductApi.Infrustructure \
  --startup-project src/ProductApi/ProductApi.Presentation

# update 
dotnet ef database update \
  --project src/ProductApi/ProductApi.Infrustructure \
  --startup-project src/ProductApi/ProductApi.Presentation
```

we need to add few things to the program.cs to view the endpoints
```cs
// ..........
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// ...........
// ...........
// ...........
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

// ...........
// ...........
app.MapControllers();
```