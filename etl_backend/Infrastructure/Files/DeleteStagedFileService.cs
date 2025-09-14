using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

public class DeleteStagedFileService : IDeleteStagedFile
{
    private readonly IFileStorage _storage;

    public DeleteStagedFileService(IFileStorage storage)
        => (_storage) = (storage);
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