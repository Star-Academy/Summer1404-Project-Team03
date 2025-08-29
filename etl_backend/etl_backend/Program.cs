using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Configurations;
using etl_backend.Application.DataFile.Services;
using etl_backend.Application.KeycalokAuth;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Services;
using etl_backend.Configuration;
using etl_backend.DbConfig;
using etl_backend.DbConfig.Abstraction;
using etl_backend.Extensions;
using etl_backend.Middlewares;
using etl_backend.Middlewares.Authentication;
using etl_backend.Repositories;
using etl_backend.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection("Keycloak"));

builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection("Storage"));

builder.Services.AddDbContextFactory<EtlDbContext>(options =>
    options.UseNpgsql(builder.Configuration["Database:ConnectionString"]));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200", "http://localhost:7252", "https://localhost:7252",
                "http://192.168.25.195:4200", "https://192.168.25.178:7252"
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
builder.Services.AddSingleton<IKeycloakLogOutUser, KeycloakLogOutUser>();
builder.Services.AddSingleton<IKeycloakTokenHandler, KeycloakTokenHandler>();
builder.Services.AddSingleton<IRoleExtractor, KeycloakRoleExtractor>();

builder.Services.AddDbContextFactory<EtlDbContext>(opt =>
    opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEtlDbContextFactory, EtlDbContextFactory>();

//repositories
builder.Services.AddScoped<IStagedFileRepository, StagedFileRepository>();
builder.Services.AddScoped<IDataTableSchemaRepository, DataTableSchemaRepository>();
builder.Services.AddScoped<IDataTableColumnRepository, DataTableColumnRepository>();

//Data saving related services
builder.Services.AddScoped<IFileStagingService, FileStagingService>();
builder.Services.AddScoped<IColumnNameSanitizer, PostgresColumnNameSanitizer>();
builder.Services.AddScoped<IColumnDefinitionBuilder, DefaultColumnDefinitionBuilder>();
builder.Services.AddScoped<ITableNameGenerator, DefaultTableNameGenerator>();
builder.Services.AddScoped<IHeaderProvider, StorageHeaderProvider>();
builder.Services.AddScoped<ISchemaRegistrationService, SchemaRegistrationService>();
builder.Services.Configure<PostgresStoreOptions>(config.GetSection("PostgresStore"));
builder.Services.AddSingleton<IIdentifierPolicy, PostgresIdentifierPolicy>();
builder.Services.AddSingleton<ITypeMapper, PostgresTypeMapper>();
builder.Services.AddSingleton<ITableNameParser, PostgresTableNameParser>();
builder.Services.AddSingleton<IDdlBuilder, PostgresDdlBuilder>();
builder.Services.AddSingleton<ITableCatalog, PostgresTableCatalog>();
builder.Services.AddSingleton<ISqlExecutor, NpgsqlSqlExecutor>();
builder.Services.AddSingleton<ICsvRowFormatter>(sp =>
{
    var opts = sp.GetRequiredService<IOptions<CsvStagingOptions>>().Value;
    return new CsvRowFormatter(opts.Delimiter, opts.QuoteChar);
});


builder.Services.AddKeycloakAuthentication(builder.Configuration);

builder.Services.AddSingleton<IKeycloakServiceAccountTokenProvider, KeycloakServiceAccountTokenProvider>();

builder.Services.AddSingleton<ISsoClient, KeycloakSsoClient>();
builder.Services.AddSingleton<IRoleManagerService, KeycloakClientRoleManagerService>();
builder.Services.AddSingleton<IKeycloakAdminClient,  KeycloakAdminClient>();

builder.Services.AddSingleton<ITokenProfileExtractor, KeycloakTokenProfileExtractor>();
builder.Services.AddSingleton<IGetAllUsersService, GetAllUsersService>();
builder.Services.AddSingleton<ICreateUserService, CreateUserService>();
builder.Services.AddSingleton<IEditUserService, EditUserService>();
builder.Services.AddSingleton<IDeleteUserService, DeleteUserService>();
builder.Services.AddSingleton<IGetUserByIdService, GetUserByIdService>();
builder.Services.AddSingleton<IEditUserRolesService, EditUserRolesService>();
builder.Services.AddSingleton<IGetRolesList, GetAllRolesService>();


builder.Services.AddAuthorization(options =>
{
    var roleSettings = builder.Configuration
        .GetSection("Authorization:Roles")
        .Get<RoleSettings>();
    
    options.AddPolicy("RequireDataAdmin", p =>
        p.RequireRole(roleSettings!.DataAdmin, roleSettings.SysAdmin));
    
    options.AddPolicy("RequireSysAdmin", p =>
        p.RequireRole(roleSettings!.SysAdmin));
    
    options.AddPolicy("RequireAnalyst", p =>
        p.RequireRole(roleSettings!.DataAnalyst, roleSettings.DataAdmin, roleSettings.SysAdmin));
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
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiExceptionMiddleware>();
app.MapControllers();



app.Run();

