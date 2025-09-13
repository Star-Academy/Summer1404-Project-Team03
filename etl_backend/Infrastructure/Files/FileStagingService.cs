using Application.Abstractions;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

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
            savedPath = await _storage.SaveFileAsync(fileStream, originalFileName, subdir ?? "");
            var size = await _storage.GetFileSizeAsync(savedPath);
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
        catch (Exception e)
        {
            // Console.WriteLine(e);
            // if (savedPath is not null)
            // {
            //     try { await _storage.DeleteFileAsync(savedPath); } catch { /* best-effort cleanup */ }
            // }
            throw;
        }
    }
    public async Task DeleteAsync(string storedFilePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(storedFilePath))
            throw new ArgumentException("Stored file path cannot be null or empty.", nameof(storedFilePath));

        try
        {
            await _storage.DeleteFileAsync(storedFilePath);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete file: {storedFilePath}", ex);
        }
    }
}
