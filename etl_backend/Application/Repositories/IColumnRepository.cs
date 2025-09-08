namespace Application.Repositories;

public interface IColumnRepository
{
    Task RenameColumnAsync(string schemaName, string tableName, string oldName, string newName, CancellationToken ct = default);
    Task DropColumnsAsync(string schemaName, string tableName, List<string> columnNames, CancellationToken ct = default);
    Task<bool> TableExistsAsync(string schemaName, string tableName, CancellationToken ct = default);
}