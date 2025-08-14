using ProductApi.Infrustructure.Data.DependencyInjection;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddInfrustructureService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

app.UserInfrustucturePolicy();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

