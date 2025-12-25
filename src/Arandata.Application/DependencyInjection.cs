using Microsoft.Extensions.DependencyInjection;
using Arandata.Application.Interfaces;
using Arandata.Application.Services;
using Arandata.Application.Mappings;

namespace Arandata.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            // Book and Loan services removed
            // Register agricultural services
            services.AddScoped<IVariedadService, VariedadService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ICosechaService, CosechaService>();

            return services;
        }
    }
}
