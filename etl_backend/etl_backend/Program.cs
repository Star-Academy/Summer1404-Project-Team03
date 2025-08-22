using etl_backend.Authentication;
using etl_backend.Authentication;
using etl_backend.Configuration;
using etl_backend.Extensions;
using etl_backend.Services.Auth;
using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Auth.keycloakService;
using etl_backend.Services.Auth.keycloakService.Abstraction;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHttpClient();
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection("Keycloak"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDev", policy =>
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

builder.Services.AddSingleton<ITokenExtractor, TokenCookieExtractor>();
builder.Services.AddSingleton<ITokenCookieService, TokenCookieService>();
builder.Services.AddSingleton<ITokenExpirationChecker, KeycloakTokenExpirationChecker>();
builder.Services.AddSingleton<IAccessTokenRefreshable, KeycloakAccessTokenRefresher>();
builder.Services.AddSingleton<IKeycloakAuthService, KeycloakAuthService>();
builder.Services.AddSingleton<IParseTokenResponse, KeycloakTokenResponseParser>();
builder.Services.AddSingleton<IKeycloakRefreshTokenRevokable, KeycloakKeycloakRefreshTokenRevoker>();
builder.Services.AddSingleton<IKeycloakTokenHandler, KeycloakTokenHandler>();
builder.Services.AddSingleton<IRoleExtractor, KeycloakRoleExtractor>();

builder.Services.AddKeycloakAuthentication(builder.Configuration);

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
app.UseCors("AllowDev");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();

