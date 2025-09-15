using Domain.Entities;

namespace Application.Services.Repositories.Abstractions;

public interface IStagedFileRepository
{
    Task<StagedFile> AddAsync(StagedFile entity, CancellationToken ct = default);
    Task<StagedFile?> GetByIdAsync(int id, CancellationToken ct = default);
    Task UpdateAsync(StagedFile entity, CancellationToken ct = default);
    Task<List<StagedFile>> ListAsync(CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}