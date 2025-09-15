namespace Application.Plugins.Abstractions;

public interface IPluginDeleter
{
    Task DeleteAsync(string id, string userId, CancellationToken ct);
}