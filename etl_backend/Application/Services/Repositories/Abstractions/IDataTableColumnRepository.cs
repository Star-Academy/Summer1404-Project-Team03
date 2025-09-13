using Domain.Entities;

namespace Application.Services.Repositories.Abstractions;

public interface IDataTableColumnRepository
{
    Task<DataTableColumn?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<DataTableColumn?> FindBySchemaAndNameAsync(int schemaId, string columnName, CancellationToken ct = default);
    Task<List<DataTableColumn>> ListBySchemaAsync(int schemaId, CancellationToken ct = default);
    Task UpdateNameAsync(int id, string newName, CancellationToken ct = default);
    Task DeleteByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}