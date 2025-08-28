using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IFileStagingService
{
    Task<StagedFile> StageAsync(
        Stream fileStream,
        string originalFileName,
        string? subdirectory = "uploads",
        CancellationToken ct = default);
}
