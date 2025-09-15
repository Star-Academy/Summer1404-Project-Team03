using etl_backend.Application.DataFile.Abstraction;
using Npgsql;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;

public sealed class PostgresTableCatalog : ITableCatalog
{
    private const string SqlExists = @"
SELECT 1
FROM   pg_catalog.pg_class c
JOIN   pg_catalog.pg_namespace n ON n.oid = c.relnamespace
WHERE  n.nspname = @schema AND c.relname = @table AND c.relkind = 'r'";

    public async Task<bool> TableExistsAsync(NpgsqlConnection conn, string schema, string table, CancellationToken ct = default)
    {
        await using var cmd = new NpgsqlCommand(SqlExists, conn);
        cmd.Parameters.AddWithValue("schema", schema);
        cmd.Parameters.AddWithValue("table", table);
        var o = await cmd.ExecuteScalarAsync(ct);
        return o is not null;
    }
}