using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Configuration;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.DataFile.Services;

public class LocalFileStorageService : IFileStorage
{
    private readonly string _mediaRoot;
    

    public LocalFileStorageService(IWebHostEnvironment env, IOptions<StorageSettings> storageSettingsOptions)
    {
        var storageRoot = storageSettingsOptions.Value.Root;
        _mediaRoot = Path.Combine(env.ContentRootPath, storageRoot);
        Directory.CreateDirectory(_mediaRoot);
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string dirPath = "")
    {
        var targetFolder = string.IsNullOrEmpty(dirPath) ? _mediaRoot : Path.Combine(_mediaRoot, dirPath);
        Directory.CreateDirectory(targetFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(targetFolder, uniqueFileName);

        using var output = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(output);

        // Return relative path 
        return Path.GetRelativePath(_mediaRoot, fullPath).Replace("\\", "/");
    }

    public Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_mediaRoot, filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }
    
    public Task<long> GetFileSizeAsync(string relativePath)
    {
        var fullPath = Path.Combine(_mediaRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        var size = File.Exists(fullPath) ? new FileInfo(fullPath).Length : 0L;
        return Task.FromResult(size);
    }
    
}
