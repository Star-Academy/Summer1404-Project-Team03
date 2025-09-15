using Domain.Entities;

namespace Infrastructure.Files.Abstractions;

public interface ILoadPreconditionsService
{
    Task<(StagedFile staged, DataTableSchema schema)> EnsureLoadableAsync(int stagedFileId, CancellationToken ct = default);
}