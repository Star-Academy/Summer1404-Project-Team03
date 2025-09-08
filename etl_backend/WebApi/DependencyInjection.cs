using Application;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using Infrastructure.Files.Abstractions;
using Microsoft.AspNetCore.Authentication;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        // FastEndpoints (scans Web layer for Endpoints & Validators)
        services.AddScoped<IStorageAppEnvironment, StorageAppEnvironment>();
        // services.AddSingleton<SystemClock>();

        // MediatR (already added in Application layer, but ensure it's included)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Register Application & Infrastructure layers
        // services.AddApplicationServices();
        // services.AddInfrastructureServices();

        return services;
    }
}