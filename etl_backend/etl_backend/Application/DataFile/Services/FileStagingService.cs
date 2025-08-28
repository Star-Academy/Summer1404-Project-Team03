using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Configuration;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;
using etl_backend.Domain.Enums;

namespace etl_backend.Application.DataFile.Services;

public sealed class FileStagingService : IFileStagingService
{
    private readonly IFileStorage _storage;
    private readonly IStagedFileRepository _repo;
    private readonly IClock _clock;

    public FileStagingService(IFileStorage storage, IStagedFileRepository repo, IClock clock)
        => (_storage, _repo, _clock) = (storage, repo, clock);

    public async Task<StagedFile> StageAsync(Stream fileStream, string originalFileName, string? subdir = "uploads", CancellationToken ct = default)
    {
        string? savedPath = null;
        try
        {
            // 1) Save file first (unique path from storage impl)
            savedPath = await _storage.SaveFileAsync(fileStream, originalFileName, subdir ?? "");
            var size = await _storage.GetFileSizeAsync(savedPath);

            // 2) Insert staging row; if this fails, delete the file (compensation)
            var staged = new StagedFile
            {
                OriginalFileName = originalFileName,
                StoredFilePath   = savedPath,
                FileSize         = size,
                UploadedAt       = _clock.UtcNow,

                Stage            = ProcessingStage.Uploaded,
                Status           = ProcessingStatus.InProgress,
                ErrorCode        = ProcessingErrorCode.None,
                ErrorMessage     = null,

                SchemaId         = null
            };

            return await _repo.AddAsync(staged, ct);
        }
        catch
        {
            if (savedPath is not null)
            {
                try { await _storage.DeleteFileAsync(savedPath); } catch { /* best-effort cleanup */ }
            }
            throw;
        }
    }
}
