namespace etl_backend.Application.DataFile.Abstraction;

public interface IFileStorage
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string dirPath = "");
    Task DeleteFileAsync(string filePath);
    Task<long> GetFileSizeAsync(string relativePath);
    Task<Stream> OpenReadAsync(string relativePath);
}
