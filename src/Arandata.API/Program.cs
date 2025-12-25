using DotNetEnv;
using Arandata.Infrastructure;
using Arandata.Application;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

var currentDir = Directory.GetCurrentDirectory();
var envPath = Path.Combine(currentDir, ".env");
if (!File.Exists(envPath))
{
    envPath = Path.Combine(currentDir, "..", "..", ".env");
}
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine($".env file loaded from: {envPath}");
    Console.WriteLine(envPath.ToString());
}
else
{
    Console.WriteLine(".env file not found.");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Crear base de datos automáticamente si no existe
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        Console.WriteLine("Creando base de datos si no existe...");
        context.Database.EnsureCreated();
        Console.WriteLine("Base de datos creada exitosamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al crear la base de datos: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
// Global exception handling middleware (normalize error responses JSON en español)
app.UseMiddleware<Arandata.API.Middleware.ExceptionHandlingMiddleware>();

// AGREGADO: Middleware de Control de Permisos por Rol y Módulo
app.UseMiddleware<Arandata.API.Middleware.PermissionMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Insert middleware to strip /api/Books from swagger JSON
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    if (path.Equals("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
    {
        var originalBody = context.Response.Body;
        using var mem = new MemoryStream();
        context.Response.Body = mem;

        await next();

        mem.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(mem, Encoding.UTF8);
        var json = await reader.ReadToEndAsync();

        // Remove the /api/Books path entry if present
        var pattern = "\"/api/Books\"\\s*:\\s*\\{[^\\{\\}]*\\}(,?)";
        var newJson = Regex.Replace(json, pattern, string.Empty, RegexOptions.Singleline);

        var bytes = Encoding.UTF8.GetBytes(newJson);
        context.Response.Body = originalBody;
        context.Response.ContentLength = bytes.Length;
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
    }
    else
    {
        await next();
    }
});

app.Run();
