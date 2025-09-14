using Domain.Entities;

namespace Application.Files.StageManyFiles.ServiceAbstractions;

public interface IAddStageFileService
{
    Task<StagedFile> StageAsync(
        Stream fileStream,
        string originalFileName,
        string? subdirectory = "uploads",
        CancellationToken ct = default);
}