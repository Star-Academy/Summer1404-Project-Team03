using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Services;
using Npgsql;

public sealed class PostgresColumnAdmin : IColumnAdmin
{
    private readonly NpgsqlDataSource _ds;
    private readonly IIdentifierPolicy _ids;
    private readonly ISqlExecutor _sql;

    public PostgresColumnAdmin(NpgsqlDataSource dataSource, IIdentifierPolicy ids, ISqlExecutor sql)
        => (_ds, _ids, _sql) = (dataSource, ids, sql);

    public async Task DropColumnsAsync(TableRef table, IEnumerable<string> columnNames, CancellationToken ct = default)
    {
        var cols = (columnNames ?? Array.Empty<string>())
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => $"DROP COLUMN IF EXISTS {_ids.QuoteIdentifier(n!)}")
            .ToArray();

        if (cols.Length == 0) return;

        var sql = $"ALTER TABLE {table.QualifiedName} {string.Join(", ", cols)};";

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await using var tx   = await conn.BeginTransactionAsync(ct);
        await _sql.ExecuteAsync((NpgsqlConnection)conn, sql, ct);
        await tx.CommitAsync(ct);
    }

    public async Task RenameColumnAsync(TableRef table, string fromName, string toName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fromName) || string.IsNullOrWhiteSpace(toName))
            throw new ArgumentException("Column names cannot be empty.");

        if (string.Equals(fromName, toName, StringComparison.Ordinal)) return; // no-op

        var sql = $"ALTER TABLE {table.QualifiedName} RENAME COLUMN {_ids.QuoteIdentifier(fromName)} TO {_ids.QuoteIdentifier(toName)};";

        await using var conn = await _ds.OpenConnectionAsync(ct);
        await _sql.ExecuteAsync((NpgsqlConnection)conn, sql, ct);
    }
}
