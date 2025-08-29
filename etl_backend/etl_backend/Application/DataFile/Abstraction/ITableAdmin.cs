using etl_backend.Application.DataFile.Dtos;
using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableAdmin
{
    Task<TableRef> EnsureTableAsync(TableSpec spec, LoadMode mode, CancellationToken ct = default);
    Task DropTableAsync(TableRef table, CancellationToken ct = default);
    Task TruncateTableAsync(TableRef table, CancellationToken ct = default);
    Task RenameTableAsync(TableRef table, string newName, CancellationToken ct = default);
}

