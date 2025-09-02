using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Configurations;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;


public sealed class TableManagementService : ITableManagementService
{
    private readonly IDataTableSchemaRepository _schemas;
    private readonly IIdentifierPolicy _ids;
    private readonly ISqlExecutor _sql;
    private readonly ITableCatalog _catalog;
    private readonly NpgsqlDataSource _ds;
    private readonly string _defaultSchema;

    public TableManagementService(
        IDataTableSchemaRepository schemas,
        IIdentifierPolicy ids,
        ISqlExecutor sql,
        ITableCatalog catalog,
        NpgsqlDataSource dataSource,
        IOptions<PostgresStoreOptions> store)
    {
        _schemas = schemas;
        _ids = ids;
        _sql = sql;
        _catalog = catalog;
        _ds = dataSource;
        _defaultSchema = store.Value.DefaultSchema ?? "public";
    }

    public async Task<IReadOnlyList<DataTableSchema>> ListAsync(bool onlyPhysical = false, CancellationToken ct = default)
    {
        var all = await _schemas.ListAsync(ct);
        if (!onlyPhysical) return all;

        var filtered = new List<DataTableSchema>(all.Count);
        await using var conn = await _ds.OpenConnectionAsync(ct);
        var npg = (NpgsqlConnection)conn;

        foreach (var s in all)
        {
            var exists = await _catalog.TableExistsAsync(npg, _defaultSchema, s.TableName, ct);
            if (exists) filtered.Add(s);
        }
        return filtered;
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

    public async Task RenameAsync(int schemaId, string newTableName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(newTableName))
            throw new ArgumentException("New table name is required.", nameof(newTableName));

        newTableName = newTableName.Trim();

        var schema = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
                     ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        var fromQualified = $"{_ids.QuoteIdentifier(_defaultSchema)}.{_ids.QuoteIdentifier(schema.TableName)}";
        var toQuotedName  = _ids.QuoteIdentifier(newTableName);

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        try
        {
            var renameSql = $"ALTER TABLE {fromQualified} RENAME TO {toQuotedName};";
            await _sql.ExecuteAsync((NpgsqlConnection)conn, renameSql, ct);

            await _schemas.UpdateTableNameAsync(schema.Id, newTableName, ct);

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}