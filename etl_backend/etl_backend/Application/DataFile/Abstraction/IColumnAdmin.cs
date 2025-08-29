using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IColumnAdmin
{
    Task DropColumnsAsync(TableRef table, IEnumerable<string> columnNames, CancellationToken ct = default);
    Task RenameColumnAsync(TableRef table, string fromName, string toName, CancellationToken ct = default);
}