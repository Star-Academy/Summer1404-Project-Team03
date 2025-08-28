using etl_backend.Domain.Entities;

namespace etl_backend.Repositories.Abstractions;

public interface IDataTableSchemaRepository
{
    Task<DataTableSchema> AddAsync(DataTableSchema schema, CancellationToken ct = default);
    Task<DataTableSchema?> GetByIdWithColumnsAsync(int id, CancellationToken ct = default);
    Task<DataTableSchema?> GetByTableNameAsync(string tableName, CancellationToken ct = default);
    Task UpdateAsync(DataTableSchema schema, CancellationToken ct = default);
}
