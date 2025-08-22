using etl_backend.Configuration;
using etl_backend.Services.Auth.keycloakAuthService.Dtos;
using etl_backend.Services.Auth.keycloakService.Abstraction;
using Microsoft.Extensions.Options;

namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakAuthService : IKeycloakAuthService
{
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IParseTokenResponse _responseTokenParser;

    public KeycloakAuthService(IOptions<KeycloakOptions> options, IHttpClientFactory httpClientFactory, IParseTokenResponse responseTokenParser)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _responseTokenParser = responseTokenParser;
    }

    public string GenerateLoginUrl(string callbackUrl)
    {
        var baseUrl = _options.AuthServerUrl!.TrimEnd('/');
        return $"{baseUrl}/realms/{_options.Realm}/protocol/openid-connect/auth" +
               $"?client_id={Uri.EscapeDataString(_options.ClientId)}" +
               $"&redirect_uri={Uri.EscapeDataString(callbackUrl)}" +
               $"&response_type=code&scope=openid";
    }

    public async Task<TokenResponseDto> ExchangeCodeForTokensAsync(string code, string redirectUri, CancellationToken ct = default)
    {
        var tokenEndpoint = $"{_options.AuthServerUrl!.TrimEnd('/')}/realms/{_options.Realm}/protocol/openid-connect/token";
        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _options.ClientId!,
            ["code"] = code,
            ["redirect_uri"] = redirectUri
        };
        
        var client = _httpClientFactory.CreateClient();
        var res = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(form), ct);
        var content = await res.Content.ReadAsStringAsync(ct);
        res.EnsureSuccessStatusCode();

        return _responseTokenParser.ParseTokenResponse(content);
    }
    
    
}