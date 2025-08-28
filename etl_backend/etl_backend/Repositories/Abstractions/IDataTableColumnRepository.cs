using etl_backend.Domain.Entities;

namespace etl_backend.Repositories.Abstractions;

public interface IDataTableColumnRepository
{
    Task AddRangeAsync(IEnumerable<DataTableColumn> columns, CancellationToken ct = default);
    Task<List<DataTableColumn>> GetBySchemaIdAsync(int schemaId, CancellationToken ct = default);
    Task DeleteBySchemaIdAsync(int schemaId, CancellationToken ct = default);
}
