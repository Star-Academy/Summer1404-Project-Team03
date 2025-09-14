using Domain.Entities;

namespace Application.Tables.ListTables.ServiceAbstractions;

public interface ITablesListService
{
    Task<IReadOnlyList<DataTableSchema>> ListAsync(CancellationToken ct = default);
}