using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Arandata.Domain.Ports.Out;
using Arandata.Infrastructure.Persistence;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Infrastructure.Persistence.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Arandata.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("BD_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};";
            Console.WriteLine($"Connection String: {connectionString}");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

            // Register DbContext as the non-generic service so repositories requesting DbContext are resolved
            services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Repositories
            services.AddScoped<Arandata.Domain.Ports.Out.IVariedadRepository, VariedadRepository>();
            services.AddScoped<Arandata.Domain.Ports.Out.IBayaBrixRepository, BayaBrixRepository>();
            services.AddScoped<Arandata.Domain.Ports.Out.ICosechaRepository, CosechaRepository>();
            services.AddScoped<Arandata.Domain.Ports.Out.ILoteRepository, LoteRepository>();
            // Eliminadas referencias a repositorios legacy Baya100 y Muestra100
            // Eliminada referencia a IMuestraBrixRepository y MuestraBrixRepository (legacy)

            return services;
        }
    }
}
