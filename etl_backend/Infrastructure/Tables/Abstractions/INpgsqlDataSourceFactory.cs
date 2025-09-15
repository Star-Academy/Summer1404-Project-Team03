using Npgsql;

namespace Infrastructure.Tables.Abstractions;

public interface INpgsqlDataSourceFactory
{
    Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken ct = default);
}