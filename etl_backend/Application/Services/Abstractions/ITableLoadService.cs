using Application.Files.Commands;

namespace Application.Abstractions;

public interface ITableLoadService
{
    Task<LoadResult> LoadAsync(int stagedFileId, ILoadPolicy policy, CancellationToken ct = default);
}