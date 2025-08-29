using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ILoadPreconditionsService
{
    Task<(StagedFile staged, DataTableSchema schema)> EnsureLoadableAsync(int stagedFileId, CancellationToken ct = default);
}