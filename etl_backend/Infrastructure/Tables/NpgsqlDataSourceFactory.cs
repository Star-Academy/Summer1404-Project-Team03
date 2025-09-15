using Infrastructure.Tables.Abstractions;
using Npgsql;

namespace Infrastructure.Tables;

public sealed class NpgsqlDataSourceFactory : INpgsqlDataSourceFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public NpgsqlDataSourceFactory(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        var conn = await _dataSource.OpenConnectionAsync(ct);
        return (NpgsqlConnection)conn;
    }
}