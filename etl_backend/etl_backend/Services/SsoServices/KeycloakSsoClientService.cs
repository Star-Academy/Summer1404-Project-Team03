using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.SsoServices;

namespace etl_backend.Services.SsoServices;

public class KeycloakSsoClient : ISsoClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _realm;

    public KeycloakSsoClient(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = config["Keycloak:BaseUrl"] ?? throw new ArgumentNullException("Keycloak:BaseUrl not configured");
        _realm = config["Keycloak:Realm"] ?? throw new ArgumentNullException("Keycloak:Realm not configured");
    }

    private string BuildUrl(string endpoint) =>
        $"{_baseUrl}/realms/{_realm}/{endpoint.TrimStart('/')}";

    private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string accessToken, object? body = null)
    {
        var request = new HttpRequestMessage(method, BuildUrl(endpoint));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private async Task HandleError(HttpResponseMessage response, string endpoint)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"{response.RequestMessage?.Method} {endpoint} failed: {response.StatusCode} - {error}");
        }
    }

    public async Task<T> GetAsync<T>(string endpoint, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, endpoint, accessToken);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, endpoint);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);

        return result ?? throw new ApplicationException($"Failed to parse response from {endpoint}");
    }

    public async Task<T?> PostAsync<T>(string endpoint, object body, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, endpoint, accessToken, body);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, endpoint);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
    }

    public async Task PostAsync(string endpoint, object body, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, endpoint, accessToken, body);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, endpoint);
    }

    public async Task PutAsync(string endpoint, object body, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Put, endpoint, accessToken, body);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, endpoint);
    }

    public async Task DeleteAsync(string endpoint, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Delete, endpoint, accessToken);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, endpoint);
    }
}
