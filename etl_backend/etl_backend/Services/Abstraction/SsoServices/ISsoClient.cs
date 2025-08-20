namespace etl_backend.Services.Abstraction;

public interface ISsoClient
{
    Task<T> GetAsync<T>(string url, string accessToken, CancellationToken cancellationToken = default);
    Task<T?> PostAsync<T>(string url, object body, string accessToken, CancellationToken cancellationToken = default);
    Task PostAsync(string url, object body, string accessToken, CancellationToken cancellationToken = default);
    Task PutAsync(string url, object body, string accessToken, CancellationToken cancellationToken = default);
    Task DeleteAsync(string url, string accessToken, CancellationToken cancellationToken = default);
}

