using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using Infrastructure.SsoServices.User.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.SsoServices.User;

public class KeycloakSsoClient : ISsoClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _realm;
    private readonly KeycloakOptions _keycloakOptions;

    public KeycloakSsoClient(IHttpClientFactory httpClientFactory, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClient = httpClientFactory.CreateClient();
        _keycloakOptions = keycloakOptions.Value;
        _baseUrl = _keycloakOptions.AuthServerUrl.TrimEnd('/');
        _realm = _keycloakOptions.Realm;
    }

    private string BuildUrl(string endpoint) =>
        $"{_baseUrl}/admin/realms/{_realm}/{endpoint.TrimStart('/')}";

    private HttpRequestMessage CreateRequest(HttpMethod method, string url, string accessToken, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new ApiException(
                $"{response.RequestMessage?.Method} {endpoint} failed"+ errorContent,
                (int)response.StatusCode,
                errorContent
            );
        }
    }
    
    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await HandleError(response, request.RequestUri?.ToString() ?? "unknown endpoint");

        return response;
    }

    private static async Task<JsonDocument?> ParseJsonContentAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        // No content at all (204 or empty body)
        if (response.Content == null ||
            response.Content.Headers.ContentLength == 0 ||
            response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (contentStream == null || contentStream.Length == 0)
        {
            return null;
        }

        return await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);
    }
    

    public async Task<JsonDocument> GetAsync(string endpoint, string accessToken, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint);
        var request = CreateRequest(HttpMethod.Get, url, accessToken);
        var response = await SendRequestAsync(request, cancellationToken);
        return await ParseJsonContentAsync(response, cancellationToken) ?? JsonDocument.Parse("{}");
    }

    public async Task<JsonDocument> PostAsync(string endpoint, object body, string accessToken, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint);
        var request = CreateRequest(HttpMethod.Post, url, accessToken, body);
        var response = await SendRequestAsync(request, cancellationToken);
        var jsonContent = await ParseJsonContentAsync(response, cancellationToken);
        var emptyJsonDoc = JsonDocument.Parse("{}");
        if (response.StatusCode == HttpStatusCode.Created && jsonContent == null)
        {
            var location = response.Headers.Location?.ToString();
            var locationRequest = CreateRequest(HttpMethod.Get, location!, accessToken);
            var locationResponse = await SendRequestAsync(locationRequest, cancellationToken);
            return await ParseJsonContentAsync(locationResponse, cancellationToken) ??  emptyJsonDoc;
        }
        return jsonContent ??  emptyJsonDoc;
    }

    public async Task<JsonDocument> PutAsync(string endpoint, object body, string accessToken, CancellationToken cancellationToken = default)
    {
        var url =  BuildUrl(endpoint);
        var request = CreateRequest(HttpMethod.Put, url, accessToken, body);
        var  response = await SendRequestAsync(request, cancellationToken);
        return await ParseJsonContentAsync(response, cancellationToken) ?? JsonDocument.Parse("{}");
    }

    public async Task<JsonDocument> DeleteAsync(string endpoint, object body, string accessToken,
        CancellationToken cancellationToken)
    {   
        var url =  BuildUrl(endpoint); 
        var request = CreateRequest(HttpMethod.Delete, url, accessToken, body);
        var  response = await SendRequestAsync(request, cancellationToken);
        return await ParseJsonContentAsync(response, cancellationToken) ?? JsonDocument.Parse("{}");
    }
}
