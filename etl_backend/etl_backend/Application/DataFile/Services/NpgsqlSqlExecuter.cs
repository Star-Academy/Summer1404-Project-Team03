using etl_backend.Application.DataFile.Abstraction;
using Npgsql;

namespace etl_backend.Application.DataFile.Services;

public sealed class NpgsqlSqlExecutor : ISqlExecutor
{
    public async Task ExecuteAsync(NpgsqlConnection conn, string sql, CancellationToken ct = default)
    {
        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}