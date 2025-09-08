using Npgsql;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableCatalog
{
    Task<bool> TableExistsAsync(NpgsqlConnection conn, string schema, string table, CancellationToken ct = default);
}