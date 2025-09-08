using Infrastructure.Files.Abstractions;

namespace WebApi;

// Web/Services/AppEnvironment.cs
public class StorageAppEnvironment : IStorageAppEnvironment
{
    private readonly IWebHostEnvironment _env;

    public StorageAppEnvironment(IWebHostEnvironment env)
    {
        _env = env;
    }

    public bool IsDevelopment => _env.IsDevelopment();
    public string ContentRootPath => _env.ContentRootPath;
}