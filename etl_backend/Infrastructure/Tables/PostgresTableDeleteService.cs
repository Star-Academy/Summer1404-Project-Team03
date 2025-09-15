using Application.Services.Repositories.Abstractions;
using Application.Tables.DeleteTable.ServiceAbstractions;
using Infrastructure.Configurations;
using Infrastructure.Files.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Tables;

public class PostgresTableDeleteService :  ITableDeleteService
{
    private readonly IDataTableSchemaRepository _schemas;
    private readonly IIdentifierPolicy _ids;
    private readonly ISqlExecutor _sql;
    private readonly NpgsqlDataSource _ds;
    private readonly string _defaultSchema;

    public PostgresTableDeleteService(
        IDataTableSchemaRepository schemas,
        IIdentifierPolicy ids,
        ISqlExecutor sql,
        NpgsqlDataSource dataSource,
        IOptions<PostgresStoreOptions> store)
    {
        _schemas = schemas;
        _ids = ids;
        _sql = sql;
        _ds = dataSource;
        _defaultSchema = store.Value.DefaultSchema ?? "public";
    }
    
    public async Task DeleteAsync(int schemaId, CancellationToken ct = default)
    {
        var schema = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
                     ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        var qualified = $"{_ids.QuoteIdentifier(_defaultSchema)}.{_ids.QuoteIdentifier(schema.TableName)}";
        var dropSql = $"DROP TABLE IF EXISTS {qualified};";

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await _sql.ExecuteAsync((NpgsqlConnection)conn, dropSql, ct);

        await _schemas.DeleteAsync(schema.Id, ct);
    }
}