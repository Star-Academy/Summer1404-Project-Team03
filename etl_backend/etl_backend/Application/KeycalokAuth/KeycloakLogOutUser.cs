using etl_backend.Application.Abstraction;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.KeycalokAuth;

public class KeycloakLogOutUser: IKeycloakLogOutUser
{
    private readonly KeycloakOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISsoClient _ssoClient;
    private readonly IKeycloakServiceAccountTokenProvider _keycloakServiceAccountTokenProvider;
    private readonly ITokenProfileExtractor  _tokenProfileExtractor;

    public KeycloakLogOutUser(IOptions<KeycloakOptions> optionsAccessor, IHttpClientFactory httpClientFactory, ISsoClient ssoClient, IKeycloakServiceAccountTokenProvider keycloakServiceAccountTokenProvider, ITokenProfileExtractor tokenProfileExtractor)
    {
        _options = optionsAccessor.Value;
        _httpClientFactory = httpClientFactory;
        _ssoClient = ssoClient;
        _keycloakServiceAccountTokenProvider = keycloakServiceAccountTokenProvider;
        _tokenProfileExtractor = tokenProfileExtractor;
    }

    public async Task<bool> LogOutAsynk(string userId, CancellationToken ct = default)
    {
        
        var endpoint = $"/users/{userId}/logout";
        
        var accessToken = await _keycloakServiceAccountTokenProvider.GetServiceAccountTokenAsync();

        await _ssoClient.PostAsync(endpoint, null, accessToken!, ct);
        // error responses is handled by _ssoClient
        return true; 
    }
}