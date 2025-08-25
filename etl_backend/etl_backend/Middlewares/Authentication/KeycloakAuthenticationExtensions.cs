using System.Security.Claims;
using etl_backend.Application.Abstraction;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace etl_backend.Authentication;

public static class KeycloakAuthenticationExtensions
{
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration config)
    {   
        var keycloakOptions = config.GetSection("Keycloak").Get<KeycloakOptions>() 
                              ?? throw new InvalidOperationException("Keycloak configuration missing.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.Authority = keycloakOptions.Authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = keycloakOptions.Audience,
                    ValidIssuer = keycloakOptions.ValidIssuer,
                    ClockSkew = keycloakOptions.ClockSkew
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async ctx => 
                    {
                        var handler = ctx.HttpContext.RequestServices.GetRequiredService<IKeycloakTokenHandler>();
                        ctx.Token = await handler.GetOrRefreshAccessTokenAsync(ctx.HttpContext, ctx.HttpContext.RequestAborted);
                    },
                    OnTokenValidated = async ctx =>
                    {
                        var identity = ctx.Principal?.Identity as ClaimsIdentity;
                        if (identity == null) return ;
                        
                        var claimsScope = config["Keycloak:RoleScope"] 
                                          ?? throw new InvalidOperationException("Keycloak configuration missing.");
                        var rolesKey = config["Keycloak:RolesKey"] 
                                          ?? throw new InvalidOperationException("Keycloak configuration missing.");
                        var extractor = ctx.HttpContext.RequestServices.GetRequiredService<IRoleExtractor>();
                        var roles = await extractor.ExtractRoles(ctx.Principal!, claimsScope, rolesKey);

                        foreach (var role in roles)
                            identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                        
                    }
                };
            });

        return services;
    }
}