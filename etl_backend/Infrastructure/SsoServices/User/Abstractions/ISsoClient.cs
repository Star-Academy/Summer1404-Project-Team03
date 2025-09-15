using System.Text.Json;

namespace Infrastructure.SsoServices.User.Abstractions;

public interface ISsoClient
{
    Task<JsonDocument> GetAsync(string url, string accessToken, CancellationToken cancellationToken = default);
    Task<JsonDocument> PostAsync(string url, object? body, string accessToken, CancellationToken cancellationToken = default);
    Task<JsonDocument> PutAsync(string url, object body, string accessToken, CancellationToken cancellationToken = default);
    Task<JsonDocument> DeleteAsync(string url, object body, string accessToken, CancellationToken cancellationToken = default);
}