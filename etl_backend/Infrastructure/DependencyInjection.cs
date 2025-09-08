using Application.Abstractions;
using Application.Files.Commands;
using Application.Repositories;
using Application.Repositories.Abstractions;
using Domain.AccessControl.Ports;
using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Services;
using Infrastructure.Configurations;
using Infrastructure.DbConfig;
using Infrastructure.DbConfig.Abstraction;
using Infrastructure.Files;
using Infrastructure.Files.Abstractions;
using Infrastructure.Files.PostgresTableServices;
using Infrastructure.Files.PostgresTableServices.HelperServices;
using Infrastructure.Repositories;
using Infrastructure.Tables;
using Infrastructure.Tables.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
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
        services.AddScoped<IEtlDbContextFactory, EtlDbContextFactory>();
        services.Configure<StorageSettings>(configuration.GetSection("Storage"));
        services.Configure<CsvStagingOptions>(configuration.GetSection("CsvStaging"));
        services.Configure<PostgresStoreOptions>(configuration.GetSection("PostgresStore"));
        var cs = configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton(sp => new NpgsqlDataSourceBuilder(cs).Build());

        // --- Repositories ---
        services.AddScoped<IStagedFileRepository, StagedFileRepository>();
        services.AddScoped<IDataTableSchemaRepository, DataTableSchemaRepository>();
        services.AddScoped<IDataTableColumnRepository, DataTableColumnRepository>();
        services.AddSingleton<INpgsqlDataSourceFactory, NpgsqlDataSourceFactory>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IColumnRepository, ColumnRepository>();
        // --- Cross-cutting ---
        // services.AddSingleton<IClock, SystemClockAdapter>();

        services.AddSingleton<IClock, SystemClock>();

        // --- Storage + CSV header/rows ---
        services.AddScoped<IFileStorage, LocalFileStorageService>();
        services.AddSingleton<ICsvHeaderReader, CsvHeaderReader>();
        services.AddScoped<IRowSourceFactory, StorageCsvRowSourceFactory>();
        // services.AddScoped<IRowSource, CsvRowSource>();


        // --- Components / helpers ---
        services.AddScoped<IColumnNameSanitizer, PostgresColumnNameSanitizer>();
        services.AddScoped<IColumnDefinitionBuilder, DefaultColumnDefinitionBuilder>();
        services.AddScoped<IColumnTypeValidator, DefaultColumnTypeValidator>();
        services.AddScoped<ITableNameGenerator, DefaultTableNameGenerator>();
        services.AddScoped<IHeaderProvider, StorageHeaderProvider>();
        services.AddScoped<ITableSpecFactory, DefaultTableSpecFactory>();
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
        services.AddScoped<IFileStagingService, FileStagingService>();
        services.AddScoped<ISchemaRegistrationService, SchemaRegistrationService>();
        services.AddScoped<ILoadPreconditionsService, LoadPreconditionsService>();
        services.AddScoped<IStagedFileStateService, StagedFileStateService>();
        services.AddScoped<ITableLoadService, TableLoadService>();
        services.AddScoped<ITableManagementService, TableManagementService>();
        services.AddScoped<IColumnAdmin, PostgresColumnAdmin>();
        services.AddScoped<IColumnManagementService, ColumnManagementService>();
        services.AddScoped<IDataTableColumnRepository, DataTableColumnRepository>();
        // services.AddScoped<ITableInfoService, TableInfoService>();

        // --- Load policy (pick a default; prefer overriding per-call) ---
        services.AddSingleton<ILoadPolicy>(_ => new LoadPolicy(
            mode: LoadMode.Append,
            dropOnFailure: false
        ));
        
        // Add this — for migrations at startup
        services.AddDbContext<EtlDbContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(cs);
            // Add this if migrations are in another assembly:
            // options.UseNpgsql(cs, o => o.MigrationsAssembly("etl_backend.Infrastructure"));
        });

        

        services.AddScoped<IRegisterAndLoadService, RegisterAndLoadService>();

        return services;
    }
}


