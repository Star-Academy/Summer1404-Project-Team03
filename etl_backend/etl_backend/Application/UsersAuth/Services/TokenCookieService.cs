using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Configuration.Enums;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.UsersAuth.Services;

public class TokenCookieService : ITokenCookieService
{
    private readonly KeycloakOptions _keycloakOptions;
    private readonly AppEnvironment _appEnvironment;

    public TokenCookieService(IConfiguration configuration, IOptions<KeycloakOptions> keycloakOptions)
    {
        var appEnvironmentName = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production";
        _appEnvironment = appEnvironmentName.ToAppEnvironment();
        _keycloakOptions = keycloakOptions.Value;
    }

    public void SetTokens(HttpResponse response, TokenResponseDto tokenResponse)
    {
        
        var now = DateTimeOffset.UtcNow;

        var sameSite = _appEnvironment switch
        {
            AppEnvironment.Development => SameSiteMode.None,
            AppEnvironment.Test        => SameSiteMode.None,
            AppEnvironment.Production  => SameSiteMode.Strict,
            _ => throw new ArgumentOutOfRangeException()
        };

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = sameSite,
            Expires = now.AddSeconds(tokenResponse.RefreshExpiresIn)
        };

        response.Cookies.Append(_keycloakOptions.AccessCookieName, tokenResponse.AccessToken, options);
        response.Cookies.Append(_keycloakOptions.RefreshCookieName, tokenResponse.RefreshToken, options);
    }

    public void RemoveTokens(HttpResponse response)
    {
        response.Cookies.Delete(_keycloakOptions.AccessCookieName);
        response.Cookies.Delete(_keycloakOptions.RefreshCookieName);
    }
}