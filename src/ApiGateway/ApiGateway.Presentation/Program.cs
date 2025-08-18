using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SharedLibrary.DependencyInjection;
using SharedLibrary.Middleware;
using MMLib.SwaggerForOcelot;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOpenApi();
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
builder.Services.AddSwaggerForOcelot(builder.Configuration);

JWTAuthenticationScheme.AddJwtAuthenticationScheme(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseCors();

app.MapOpenApi();

// Attach custom middleware before Ocelot
app.UseMiddleware<AttachSignatureToRequest>();



// Ocelot should be last in the pipeline
await app.UseOcelot();

app.UseSwaggerForOcelotUI(opt =>
{
    // This internal endpoint serves the transformed docs list:
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.Run();

internal class AttachSignatureToRequest
{
    private readonly RequestDelegate _next;

    public AttachSignatureToRequest(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers["Api-Gateway"] = "Signed";
        await _next(context);
    }
}
