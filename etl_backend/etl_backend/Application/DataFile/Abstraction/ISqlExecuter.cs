using Npgsql;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ISqlExecutor
{
    Task ExecuteAsync(NpgsqlConnection conn, string sql, CancellationToken ct = default);
}