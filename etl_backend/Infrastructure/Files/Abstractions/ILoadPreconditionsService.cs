using Domain.Entities;

namespace Infrastructure.Files.Abstractions;

public interface ILoadPreconditionsService
{
    Task<(StagedFile staged, DataTableSchema schema)> EnsureLoadableAsync(Guid stagedFileId, CancellationToken ct = default);
}