using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableManagementService
{
    Task<IReadOnlyList<DataTableSchema>> ListAsync(bool onlyPhysical = false, CancellationToken ct = default);
    Task RenameAsync(int schemaId, string newTableName, CancellationToken ct = default);
    Task DeleteAsync(int schemaId, CancellationToken ct = default);
}