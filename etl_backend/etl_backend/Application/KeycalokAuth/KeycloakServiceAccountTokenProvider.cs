using System.Text.Json;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Services.SsoServices.Exceptions;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.KeycalokAuth;

public class KeycloakServiceAccountTokenProvider : IKeycloakServiceAccountTokenProvider
{
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public KeycloakServiceAccountTokenProvider(IHttpClientFactory httpClientFactory, IOptions<KeycloakOptions> options)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> GetServiceAccountTokenAsync(CancellationToken ct = default)
    {
        var tokenEndpoint = $"{_options.AuthServerUrl!.TrimEnd('/')}/realms/{_options.Realm}/protocol/openid-connect/token";

        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _options.ClientId!,
            ["client_secret"] = _options.ClientSecret!
        };

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(form), ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"{response.RequestMessage?.Method} {tokenEndpoint} failed",
                (int)response.StatusCode,
                content);
        }

        var json = JsonDocument.Parse(content).RootElement;

        if (json.TryGetProperty("access_token", out var tokenEl))
        {
            return tokenEl.GetString();
        }
        else
        {
            throw new Exception("Failed to get access token");
        }
    }
}