namespace Application.Files.DeleteStagedFile.ServiceAbstractions;

public interface IDeleteStagedFile
{
    Task DeleteAsync(string storedFilePath, CancellationToken ct = default);
}