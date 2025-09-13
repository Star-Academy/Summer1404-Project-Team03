using Application.Enums;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Configurations;
using Infrastructure.Files.Abstractions;
using Infrastructure.Files.Dtos;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Files.PostgresTableServices;

public sealed class PostgresTableAdmin : ITableAdmin
{
    private readonly NpgsqlDataSource _ds;
    private readonly IIdentifierPolicy _ids;
    private readonly ITableNameParser _parser;
    private readonly IDdlBuilder _ddl;
    private readonly ITableCatalog _catalog;
    private readonly ISqlExecutor _sql;
    private readonly string _defaultSchema;

    public PostgresTableAdmin(
        NpgsqlDataSource dataSource,
        IIdentifierPolicy ids,
        ITableNameParser parser,
        IDdlBuilder ddl,
        ITableCatalog catalog,
        ISqlExecutor sql,
        IOptions<PostgresStoreOptions> opts)
    {
        _ds = dataSource;
        _ids = ids;
        _parser = parser;
        _ddl = ddl;
        _catalog = catalog;
        _sql = sql;
        _defaultSchema = (opts.Value.DefaultSchema ?? "public").Trim();
    }

    public async Task<TableRef> EnsureTableAsync(TableSpec spec, LoadMode mode, CancellationToken ct = default)
    {
        var (schema, table) = _parser.Parse(spec.Name, _defaultSchema);
        var qualified = _ids.QualifyTable(schema, table);

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await using var tx   = await conn.BeginTransactionAsync(ct);

        var exists = await _catalog.TableExistsAsync((NpgsqlConnection)conn, schema, table, ct);

        switch (mode)
        {
            case LoadMode.FailIfExists when exists:
                throw new InvalidOperationException($"Table {schema}.{table} already exists.");

            case LoadMode.Replace:
                if (exists)
                    await _sql.ExecuteAsync((NpgsqlConnection)conn, $"DROP TABLE {qualified};", ct);
                await _sql.ExecuteAsync((NpgsqlConnection)conn, _ddl.BuildCreateTableSql(qualified, spec.Columns), ct);
                break;

            case LoadMode.Append:
                if (!exists)
                    await _sql.ExecuteAsync((NpgsqlConnection)conn, _ddl.BuildCreateTableSql(qualified, spec.Columns, ifNotExists: true), ct);
                break;

            case LoadMode.FailIfExists: 
                await _sql.ExecuteAsync((NpgsqlConnection)conn, _ddl.BuildCreateTableSql(qualified, spec.Columns), ct);
                break;
        }

        await tx.CommitAsync(ct);
        return new TableRef(qualified);
    }

    public async Task DropTableAsync(TableRef table, CancellationToken ct = default)
    {
        await using var conn = await _ds.OpenConnectionAsync(ct);
        await _sql.ExecuteAsync((NpgsqlConnection)conn, $"DROP TABLE IF EXISTS {table.QualifiedName};", ct);
    }

    public async Task TruncateTableAsync(TableRef table, CancellationToken ct = default)
    {
        await using var conn = await _ds.OpenConnectionAsync(ct);
        await _sql.ExecuteAsync((NpgsqlConnection)conn, $"TRUNCATE TABLE {table.QualifiedName};", ct);
    }

    public async Task RenameTableAsync(TableRef table, string newName, CancellationToken ct = default)
    {
        // Support renaming and/or moving to another schema
        var (curSchema, curTable) = _parser.Parse(table.QualifiedName, _defaultSchema); // QualifiedName already quoted; we only need parts for checking
        var (newSchema, newTable) = _parser.Parse(newName, _defaultSchema);

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await using var tx   = await conn.BeginTransactionAsync(ct);

        // If moving across schemas and target exists, fail early
        var targetExists = await _catalog.TableExistsAsync((NpgsqlConnection)conn, newSchema, newTable, ct);
        if (targetExists)
            throw new InvalidOperationException($"Target table {newSchema}.{newTable} already exists.");

        var curQualified = _ids.QualifyTable(curSchema, curTable);

        // Move schema first if needed
        if (!string.Equals(curSchema, newSchema, StringComparison.Ordinal))
        {
            await _sql.ExecuteAsync((NpgsqlConnection)conn,
                $"ALTER TABLE {curQualified} SET SCHEMA {_ids.QuoteIdentifier(newSchema)};", ct);
            curSchema = newSchema; // qualified name changes after move
            curQualified = _ids.QualifyTable(curSchema, curTable);
        }

        // Then rename inside the (possibly new) schema
        if (!string.Equals(curTable, newTable, StringComparison.Ordinal))
        {
            await _sql.ExecuteAsync((NpgsqlConnection)conn,
                $"ALTER TABLE {curQualified} RENAME TO {_ids.QuoteIdentifier(newTable)};", ct);
        }

        await tx.CommitAsync(ct);
    }
}
