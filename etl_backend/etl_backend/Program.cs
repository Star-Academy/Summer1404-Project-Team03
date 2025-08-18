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



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
        };
        
        o.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var identity = ctx.Principal!.Identity as ClaimsIdentity;

                // Extract the token from the header
                var authHeader = ctx.Request.Headers["Authorization"].ToString();
                var token = authHeader.StartsWith("Bearer ") 
                    ? authHeader["Bearer ".Length..].Trim() 
                    : authHeader;
                Console.WriteLine(token);
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                Console.WriteLine(jwtToken);
                
                var realmAccessString = jwtToken.Payload["realm_access"]?.ToString();
                if (!string.IsNullOrEmpty(realmAccessString))
                {
                    var realmAccess = JObject.Parse(realmAccessString);
                    var roles = realmAccess["roles"] as JArray;
                    if (roles != null)
                    {
                        foreach (var role in roles)
                        {
                            identity!.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                            Console.WriteLine(role.ToString());
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };

    });
    
builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
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

app.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
{
    return claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
}).RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}