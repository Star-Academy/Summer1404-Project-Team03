using Application.Common.Services.Abstractions;
using Application.Dtos;
using Application.Files.Commands;
using Application.Services.Abstractions;

namespace Application.Abstractions;

public interface ITableLoadService
{
    Task<LoadResult> LoadAsync(Guid stagedFileId, ILoadPolicy policy, CancellationToken ct = default);
}