using Application.Abstractions;
using Application.Common.Configurations;
using Application.Common.Services.Abstractions;
using Application.Enums;
using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Application.Files.StageManyFiles.ServiceAbstractions;
using Application.Services.Abstractions;
using Application.Services.Repositories.Abstractions;
using Application.Tables.DeleteTable.ServiceAbstractions;
using Application.Tables.ListTables.ServiceAbstractions;
using Application.Tables.RenameTable.ServiceAbstractions;
using Application.Users.CreateUser.ServiceAbstractions;
using Application.Users.DeleteUser.ServiceAbstractions;
using Application.Users.EditUser.ServiceAbstractions;
using Application.Users.GetAllRoles.ServiceAbstractions;
using Application.Users.GetUserById.ServiceAbstractions;
using Application.Users.ListUsers;
using Domain.Entities;
using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Services;
using Infrastructure.Configurations;
using Infrastructure.DbConfig;
using Infrastructure.DbConfig.Abstraction;
using Infrastructure.Dtos;
using Infrastructure.Files;
using Infrastructure.Files.Abstractions;
using Infrastructure.Files.PostgresTableServices;
using Infrastructure.Files.PostgresTableServices.HelperServices;
using Infrastructure.Identity;
using Infrastructure.Identity.Abstractions;
using Infrastructure.Repositories;
using Infrastructure.SsoServices.Admin;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.User;
using Infrastructure.SsoServices.User.Abstractions;
using Infrastructure.Tables;
using Infrastructure.Tables.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using IColumnDefinitionBuilder = Application.Abstractions.IColumnDefinitionBuilder;
using ILoadPolicyFactory = Application.Abstractions.ILoadPolicyFactory;
using SystemClock = Infrastructure.Configurations.SystemClock;

namespace Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<EtlDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddSingleton<IEtlDbContextFactory, EtlDbContextFactory>();
        services.Configure<StorageSettings>(configuration.GetSection("Storage"));
        services.Configure<CsvStagingOptions>(configuration.GetSection("CsvStaging"));
        services.Configure<PostgresStoreOptions>(configuration.GetSection("PostgresStore"));
        var cs = configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton(sp => new NpgsqlDataSourceBuilder(cs).Build());

        // --- Repositories ---
        services.AddSingleton<IStagedFileRepository, StagedFileRepository>();
        services.AddSingleton<IDataTableSchemaRepository, DataTableSchemaRepository>();
        services.AddSingleton<IDataTableColumnRepository, DataTableColumnRepository>();
        services.AddSingleton<INpgsqlDataSourceFactory, NpgsqlDataSourceFactory>();
        services.AddSingleton<ITableRepository, TableRepository>();
        services.AddSingleton<IColumnRepository, ColumnRepository>();
        // --- Cross-cutting ---
        // services.AddSingleton<IClock, SystemClockAdapter>();

        services.AddSingleton<IClock, SystemClock>();

        // --- Storage + CSV header/rows ---
        services.AddSingleton<IFileStorage, LocalFileStorageService>();
        services.AddSingleton<ICsvHeaderReader, CsvHeaderReader>();
        services.AddSingleton<IRowSourceFactory, StorageCsvRowSourceFactory>();
        // services.AddScoped<IRowSource, CsvRowSource>();


        // --- Components / helpers ---
        services.AddSingleton<IColumnNameSanitizer, PostgresColumnNameSanitizer>();
        services.AddSingleton<IColumnDefinitionBuilder, DefaultColumnDefinitionBuilder>();
        services.AddSingleton<IColumnTypeValidator, DefaultColumnTypeValidator>();
        services.AddSingleton<ITableNameGenerator, DefaultTableNameGenerator>();
        services.AddSingleton<IHeaderProvider, StorageHeaderProvider>();
        services.AddSingleton<ITableSpecFactory, DefaultTableSpecFactory>();
        services.AddSingleton<ILoadPolicyFactory, LoadPolicyFactory>();

        services.AddSingleton<IIdentifierPolicy, PostgresIdentifierPolicy>();
        services.AddSingleton<ITypeMapper, PostgresTypeMapper>();
        services.AddSingleton<ITableNameParser, PostgresTableNameParser>();
        services.AddSingleton<IDdlBuilder, PostgresDdlBuilder>();
        services.AddSingleton<ITableCatalog, PostgresTableCatalog>();
        services.AddSingleton<ISqlExecutor, NpgsqlSqlExecutor>();
        services.AddSingleton<ICsvRowFormatter>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<CsvStagingOptions>>().Value;
            return new CsvRowFormatter(opts.Delimiter, opts.QuoteChar);
        });

        // --- Provider adapters (Postgres) ---
        services.AddSingleton<ITableAdmin, PostgresTableAdmin>();
        services.AddSingleton<IColumnAdmin, PostgresColumnAdmin>();
        services.AddSingleton<IDataWrite, PostgresBulkWriter>();

        // --- Use-cases / orchestrators ---
        services.AddSingleton<IRegisterAndLoadService, RegisterAndLoadService>();
        services.AddSingleton<ISchemaRegistrationService, SchemaRegistrationService>();
        services.AddSingleton<ILoadPreconditionsService, LoadPreconditionsService>();
        services.AddSingleton<IStagedFileStateService, StagedFileStateService>();
        services.AddSingleton<ITableLoadService, TableLoadService>();
        services.AddSingleton<IDataTableColumnRepository, DataTableColumnRepository>();
        services.AddSingleton<ITableDeleteService, PostgresTableDeleteService>();
        services.AddSingleton<ITableRenameService, PostgresTableRenameService>();
        services.AddSingleton<ITablesListService, PostgresTablesListService>();
        services.AddSingleton<IAddStageFileService, AddStageFile>();
        services.AddSingleton<IDeleteStagedFile, DeleteStagedFileService>();

        services.AddSingleton<ILoadPolicy>(_ => new LoadPolicy(
            mode: LoadMode.Append,
            dropOnFailure: false
        ));
        
        services.AddDbContext<EtlDbContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(cs);
            // options.UseNpgsql(cs, o => o.MigrationsAssembly("etl_backend.Infrastructure"));
        });
        services.Configure<ColumnTypeConfiguration>(configuration.GetSection("ColumnTypeConfiguration"));
        services.AddSingleton<ITypeCatalogService, TypeCatalogService>();
        services.AddSingleton<IColumnTypeValidator, DefaultColumnTypeValidator>();
        
        // --- Auth ---
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IKeycloakAuthService, KeycloakAuthService>();
        services.AddSingleton<ITokenExtractor, TokenCookieExtractor>();
        services.AddSingleton<ITokenCookieService, TokenCookieService>();
        services.AddSingleton<ITokenExpirationChecker, KeycloakTokenExpirationChecker>();
        services.AddSingleton<IAccessTokenRefreshable, KeycloakAccessTokenRefresher>();
        services.AddSingleton<IKeycloakAuthService, KeycloakAuthService>();
        services.AddSingleton<IParseTokenResponse, KeycloakTokenResponseParser>();
        services.AddSingleton<IKeycloakLogOutUser, KeycloakLogOutUser>();
        services.AddSingleton<IKeycloakTokenHandler, KeycloakTokenHandler>();
        services.AddSingleton<IRoleExtractor, KeycloakRoleExtractor>();
        services.AddSingleton<ISsoClient, KeycloakSsoClient>();
        services.AddSingleton<IRoleManagerService, KeycloakClientRoleManagerService>();
        services.AddSingleton<IKeycloakServiceAccountTokenProvider, KeycloakServiceAccountTokenProvider>();
        services.Configure<KeycloakOptions>(
            configuration.GetSection("Keycloak"));

        
        // --- Admin ---
        services.AddSingleton<IUserRoleManagementService,  SsoServices.Admin.UserRoleManagementService>();
        services.AddSingleton<ITokenProfileExtractor, KeycloakTokenProfileExtractor>();
        services.AddSingleton<ICreateUser, CreateUserService>();
        services.AddSingleton<IAdminEditUserService, EditUserService>();
        services.AddSingleton<IDeleteUserService, DeleteUserService>();
        services.AddSingleton<IGetUserByIdService, GetUserByIdService>();
        services.AddSingleton<IListUsersService, GetAllUsersService>();
        services.AddSingleton<IGetAllRoles, GetAllRolesService>();

        return services;
    }
}


