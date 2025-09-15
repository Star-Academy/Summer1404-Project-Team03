namespace Infrastructure.Files.Abstractions;

public interface IStorageAppEnvironment
{
    bool IsDevelopment { get; }
    string ContentRootPath { get; }
}