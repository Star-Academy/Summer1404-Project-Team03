using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableLoadService
{
    Task<LoadResult> LoadAsync(int stagedFileId, ILoadPolicy policy, CancellationToken ct = default);
}