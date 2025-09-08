using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        // services.AddScoped<Files.Services.IRegisterAndLoadService, Infrastructure.Files.RegisterAndLoadService>();

        return services;
    }
}