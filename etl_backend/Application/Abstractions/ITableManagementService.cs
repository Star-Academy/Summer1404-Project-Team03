using Domain.Entities;

namespace Application.Abstractions;

public interface ITableManagementService
{
    Task<IReadOnlyList<DataTableSchema>> ListAsync(CancellationToken ct = default);
    Task RenameAsync(int schemaId, string newTableName, CancellationToken ct = default);
    Task DeleteAsync(int schemaId, CancellationToken ct = default);
}
