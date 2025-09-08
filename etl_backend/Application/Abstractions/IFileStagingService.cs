using Domain.Entities;

namespace Application.Abstractions;

public interface IFileStagingService
{
    Task<StagedFile> StageAsync(
        Stream fileStream,
        string originalFileName,
        string? subdirectory = "uploads",
        CancellationToken ct = default);
    Task DeleteAsync(string storedFilePath, CancellationToken ct = default);
}