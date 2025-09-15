using Application;
using Application.Common.Authorization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using Infrastructure.Dtos;
using Infrastructure.Files.Abstractions;
using Microsoft.AspNetCore.Authentication;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageAppEnvironment, StorageAppEnvironment>();
        // services.AddSingleton<SystemClock>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.RequireSysAdmin, policy =>
                policy.RequireRole(AppRoles.SysAdmin));

            options.AddPolicy(AppPolicies.RequireDataAdmin, policy =>
                policy.RequireRole(AppRoles.DataAdmin));

            options.AddPolicy(AppPolicies.RequireAnalyst, policy =>
                policy.RequireRole(AppRoles.Analyst));

            options.AddPolicy(AppPolicies.RequireAtLeastAnalyst, policy =>
                policy.RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin));

            options.AddPolicy(AppPolicies.RequireAtLeastDataAdmin, policy =>
                policy.RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin));
        });
        services.AddHttpClient();
        
        // services.AddApplicationServices();
        // services.AddInfrastructureServices();

        return services;
    }
}