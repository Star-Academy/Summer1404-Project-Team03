using Domain.Entities;

namespace Application.Services.Repositories.Abstractions;

public interface IDataTableSchemaRepository
{
    Task<List<DataTableSchema>> ListAsync(CancellationToken ct = default);

    Task<DataTableSchema?> GetByIdWithColumnsAsync(int id, CancellationToken ct = default);

    Task AddAsync(DataTableSchema schema, CancellationToken ct = default);

    Task UpdateAsync(DataTableSchema schema, CancellationToken ct = default);

    Task UpdateTableNameAsync(int id, string newTableName, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}