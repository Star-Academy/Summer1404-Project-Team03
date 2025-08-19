using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using etl_backend.Configuration;
using etl_backend.Extensions;
using etl_backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddHttpClient();
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection("Keycloak"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200", "http://localhost:7252", "https://localhost:7252" 
                )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            ;
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        // 👇 This points to your Keycloak realm, it includes the JWKS endpoint
        options.Authority = $"{builder.Configuration["Keycloak:ServerUrl"]}/realms/{builder.Configuration["Keycloak:Realm"]}";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // 👇 match your client id in Keycloak
            ValidAudience = builder.Configuration["Keycloak:Audience"],
            ValidIssuer = $"{builder.Configuration["Keycloak:ServerUrl"]}/realms/{builder.Configuration["Keycloak:Realm"]}"
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                // Read token from cookie
                if (ctx.Request.Cookies.ContainsKey("access_token"))
                {
                    ctx.Token = ctx.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            },
            OnTokenValidated =  ctx =>
            {
                Console.WriteLine(ctx.SecurityToken);
                var identity = ctx.Principal!.Identity as ClaimsIdentity;
                
                var realmAccessClaim = ctx.Principal.FindFirst("realm_access");
                if (realmAccessClaim != null)
                {
                    var realmAccess = JObject.Parse(realmAccessClaim.Value);
                    var roles = realmAccess["roles"]?.ToObject<List<string>>() ?? new();
                
                    foreach (var role in roles)
                    {
                        // Map to ASP.NET Core roles
                        identity!.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    var roleSettings = builder.Configuration
        .GetSection("Authorization:Roles")
        .Get<RoleSettings>();

    options.AddPolicy("RequireDataAdmin",
        p => p.RequireRole(roleSettings!.DataAdmin));

    options.AddPolicy("RequireSysAdmin",
        p => p.RequireRole(roleSettings!.SysAdmin));

    options.AddPolicy("RequireAnalyst",
        p => p.RequireRole(roleSettings!.Analyst));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
{
    return claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
}).RequireAuthorization();

app.Run();

