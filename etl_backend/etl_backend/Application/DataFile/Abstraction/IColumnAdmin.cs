using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IColumnAdmin
{
    Task RenameAsync(Npgsql.NpgsqlConnection conn, string schema, string table, string oldName, string newName, CancellationToken ct = default);
    Task DropAsync(Npgsql.NpgsqlConnection conn, string schema, string table, IReadOnlyCollection<string> columnNames, CancellationToken ct = default);
}