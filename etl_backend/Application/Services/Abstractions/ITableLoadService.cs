using Application.Dtos;
using Application.Files.Commands;
using Application.Services.Abstractions;

namespace Application.Abstractions;

public interface ITableLoadService
{
    Task<LoadResult> LoadAsync(int stagedFileId, ILoadPolicy policy, CancellationToken ct = default);
}