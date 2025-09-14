using Domain.Entities;

namespace Application.Services.Repositories.Abstractions;

public interface IDataTableColumnRepository
{
    Task UpdateNameAsync(int id, string newName, CancellationToken ct = default);
    Task DeleteByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}