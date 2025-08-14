using OrderApi.Infrustructure.DependencyInjection;
using Scalar.AspNetCore;
using OrderApi.Application.DependencyInjection;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// OpenAPI generation
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddSwaggerGen();

// Your custom services
builder.Services.AddInfructrutureService(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // IMPORTANT: Expose OpenAPI document
    app.MapOpenApi();

    // Swagger middleware
    app.UseSwagger();
    app.UseSwaggerUI();

    // Scalar UI
    app.MapScalarApiReference(options =>
    {
        options.Title = "Order API Docs";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
