using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Services.SsoServices.Exceptions;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.KeycalokAuth;

public class KeycloakAuthService : IKeycloakAuthService
{
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IParseTokenResponse _responseTokenParser;
    private readonly ISsoClient _ssoClient;
    private readonly IKeycloakServiceAccountTokenProvider _keycloakTokenProvider;

    public KeycloakAuthService(IOptions<KeycloakOptions> options, IHttpClientFactory httpClientFactory, IParseTokenResponse responseTokenParser, ISsoClient ssoClient, IKeycloakServiceAccountTokenProvider keycloakTokenProvider)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _responseTokenParser = responseTokenParser;
        _ssoClient = ssoClient;
        _keycloakTokenProvider = keycloakTokenProvider;
    }

    public string GenerateLoginUrl(string callbackUrl)
    {
        var baseUrl = _options.AuthServerUrlPublic!.TrimEnd('/');
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
            ["redirect_uri"] = redirectUri,
            ["client_secret"] = _options.ClientSecret ?? throw new NullReferenceException(_options.ClientSecret),
        };
        
        var client = _httpClientFactory.CreateClient();
        var res = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(form), ct);
        var content = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
        {
            throw new ApiException($"{res.RequestMessage?.Method} {tokenEndpoint} failed",
                (int)res.StatusCode,
                content);
        }

        return _responseTokenParser.ParseTokenResponse(content);
    }
    
    public string GenerateChangePasswordUrlPage()
    {
        var baseUrl = _options.AuthServerUrlPublic!.TrimEnd('/');
        return $"{baseUrl}/realms/{_options.Realm}/account/account-security/signing-in/";
    }
    
}