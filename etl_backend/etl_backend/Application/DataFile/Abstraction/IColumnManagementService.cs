using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IColumnManagementService
{
    Task<IReadOnlyList<DataTableColumn>> ListAsync(int schemaId, CancellationToken ct = default);
    Task RenameAsync(int schemaId, int columnId, string newName, CancellationToken ct = default);
    Task DropAsync(int schemaId, IReadOnlyCollection<int> columnIds, CancellationToken ct = default);
}