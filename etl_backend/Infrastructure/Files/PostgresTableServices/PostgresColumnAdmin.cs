using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files.PostgresTableServices;

public sealed class PostgresColumnAdmin : IColumnAdmin
{
    private readonly IIdentifierPolicy _ids;
    private readonly ISqlExecutor _sql;

    public PostgresColumnAdmin(IIdentifierPolicy ids, ISqlExecutor sql)
    {
        _ids = ids;
        _sql = sql;
    }

    public async Task RenameAsync(Npgsql.NpgsqlConnection conn, string schema, string table, string oldName, string newName, CancellationToken ct = default)
    {
        var qSchema = _ids.QuoteIdentifier(schema);
        var qTable  = _ids.QuoteIdentifier(table);
        var qOld    = _ids.QuoteIdentifier(oldName);
        var qNew    = _ids.QuoteIdentifier(newName);

        var sql = $"ALTER TABLE {qSchema}.{qTable} RENAME COLUMN {qOld} TO {qNew};";
        await _sql.ExecuteAsync(conn, sql, ct);
    }

    public async Task DropAsync(Npgsql.NpgsqlConnection conn, string schema, string table, IReadOnlyCollection<string> columnNames, CancellationToken ct = default)
    {
        if (columnNames.Count == 0) return;

        var qSchema = _ids.QuoteIdentifier(schema);
        var qTable  = _ids.QuoteIdentifier(table);

        var drops = string.Join(", ", columnNames.Select(n => $"DROP COLUMN IF EXISTS {_ids.QuoteIdentifier(n)}"));
        var sql = $"ALTER TABLE {qSchema}.{qTable} {drops};";
        await _sql.ExecuteAsync(conn, sql, ct);
    }
}
