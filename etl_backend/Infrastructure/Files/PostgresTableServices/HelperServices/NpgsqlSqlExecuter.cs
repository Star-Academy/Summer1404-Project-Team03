using Infrastructure.Files.Abstractions;
using Npgsql;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;

public sealed class NpgsqlSqlExecutor : ISqlExecutor
{
    public async Task ExecuteAsync(NpgsqlConnection conn, string sql, CancellationToken ct = default)
    {
        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}