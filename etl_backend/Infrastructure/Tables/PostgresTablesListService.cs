using Application.Services.Repositories.Abstractions;
using Application.Tables.ListTables.ServiceAbstractions;
using Domain.Entities;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Tables;

public class PostgresTablesListService :  ITablesListService
{
    private readonly IDataTableSchemaRepository _schemas;
    private readonly ITableCatalog _catalog;
    private readonly NpgsqlDataSource _ds;
    private readonly string _defaultSchema;

    public PostgresTablesListService(
        IDataTableSchemaRepository schemas,
        ITableCatalog catalog,
        NpgsqlDataSource dataSource,
        IOptions<PostgresStoreOptions> store)
    {
        _schemas = schemas;
        _catalog = catalog;
        _ds = dataSource;
        _defaultSchema = store.Value.DefaultSchema ?? "public";
    }
    public async Task<IReadOnlyList<DataTableSchema>> ListAsync(CancellationToken ct = default)
    {
        var all = await _schemas.ListAsync(ct);

        var physical = new List<DataTableSchema>(all.Count);
        await using var conn = await _ds.OpenConnectionAsync(ct);
        var npg = (NpgsqlConnection)conn;

        foreach (var s in all)
        {
            if (await _catalog.TableExistsAsync(npg, _defaultSchema, s.TableName, ct))
                physical.Add(s);
        }
        return physical;
    }
}