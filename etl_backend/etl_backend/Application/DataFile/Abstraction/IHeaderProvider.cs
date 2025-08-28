using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IHeaderProvider
{
    Task<IReadOnlyList<string>> GetAsync(StagedFile staged, CancellationToken ct = default);
}