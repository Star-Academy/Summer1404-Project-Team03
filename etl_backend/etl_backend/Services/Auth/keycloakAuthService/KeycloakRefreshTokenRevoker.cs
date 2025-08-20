using etl_backend.Configuration;
using etl_backend.Services.Auth.keycloakAuthService.Abstraction;

namespace etl_backend.Services.Auth.keycloakAuthService;

public class KeycloakRefreshTokenRevoker: IRefreshTokenRevokable
{
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public KeycloakRefreshTokenRevoker(KeycloakOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> RevokeTokenAsynk(string refreshToken, CancellationToken ct = default)
    {
        var url = $"{_options.AuthServerUrl}/realms/{_options.Realm}/protocol/openid-connect/logout";

        var formData = new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["token"] = refreshToken,
            ["token_type_hint"] = "refresh_token"
        };
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(formData));

        return response.IsSuccessStatusCode;
    }
}