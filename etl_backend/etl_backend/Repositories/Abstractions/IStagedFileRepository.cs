using etl_backend.Domain.Entities;

namespace etl_backend.Repositories.Abstractions;

public interface IStagedFileRepository
{
    Task<StagedFile> AddAsync(StagedFile entity, CancellationToken ct = default);
    Task<StagedFile?> GetByIdAsync(int id, CancellationToken ct = default);
    Task UpdateAsync(StagedFile entity, CancellationToken ct = default);
}