using etl_backend.Configuration;
using etl_backend.Services.Auth.keycloakAuthService.Dtos;
using etl_backend.Services.Auth.keycloakService.Abstraction;
using Microsoft.Extensions.Options;

namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakAccessTokenRefresher: IAccessTokenRefreshable
{
    
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IParseTokenResponse _parseTokenResponse;

    public KeycloakAccessTokenRefresher(IOptions<KeycloakOptions> options, IHttpClientFactory httpClientFactory, IParseTokenResponse parseTokenResponse)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _parseTokenResponse = parseTokenResponse;
    }
    
    public async Task<TokenResponseDto?> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(refreshToken)) return null;

        var tokenEndpoint = $"{_options.AuthServerUrl!.TrimEnd('/')}/realms/{_options.Realm}/protocol/openid-connect/token";
        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = _options.ClientId!,
            ["refresh_token"] = refreshToken
        };

        var client = _httpClientFactory.CreateClient();
        var res = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(form), ct);
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await res.Content.ReadAsStringAsync(ct);
        return _parseTokenResponse.ParseTokenResponse(content);
    }
}