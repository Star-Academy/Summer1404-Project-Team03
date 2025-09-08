namespace Infrastructure.Files.Abstractions;

public interface IColumnAdmin
{
    Task RenameAsync(Npgsql.NpgsqlConnection conn, string schema, string table, string oldName, string newName, CancellationToken ct = default);
    Task DropAsync(Npgsql.NpgsqlConnection conn, string schema, string table, IReadOnlyCollection<string> columnNames, CancellationToken ct = default);
}