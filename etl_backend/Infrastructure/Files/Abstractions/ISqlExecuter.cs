using Npgsql;

namespace Infrastructure.Files.Abstractions;

public interface ISqlExecutor
{
    Task ExecuteAsync(NpgsqlConnection conn, string sql, CancellationToken ct = default);
}