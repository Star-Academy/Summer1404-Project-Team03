using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace etl_backend.Authentication
{
    public static class KeycloakAuthenticationExtensions
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var serverUrl = config["Keycloak:ServerUrl"];
            serverUrl = "http://localhost:8080";
            var realm = config["Keycloak:Realm"];
            var audience = config["Keycloak:Audience"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.Authority = $"{serverUrl}/realms/{realm}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidAudience = audience,
                        ValidIssuer = $"{serverUrl}/realms/{realm}"
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            // read token from HttpOnly cookie
                            if (ctx.Request.Cookies.TryGetValue("access_token", out var token))
                            {
                                ctx.Token = token;
                            }
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = ctx =>
                        {
                            var identity = ctx.Principal?.Identity as ClaimsIdentity;
                            if (identity == null) return Task.CompletedTask;

                            // extract realm_access.roles
                            var realmAccessClaim = ctx.Principal.FindFirst("realm_access");
                            if (realmAccessClaim != null)
                            {
                                var realmAccess = JObject.Parse(realmAccessClaim.Value);
                                var roles = realmAccess["roles"]?.ToObject<List<string>>() ?? new();

                                foreach (var role in roles)
                                {
                                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
