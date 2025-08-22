using System.Text.Json;
using etl_backend.Services.Auth.keycloakAuthService.Dtos;
using etl_backend.Services.Auth.keycloakService.Abstraction;

namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakTokenResponseParser: IParseTokenResponse
{
    public TokenResponseDto ParseTokenResponse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        return new TokenResponseDto
        {
            AccessToken = root.GetProperty("access_token").GetString()!,
            RefreshToken = root.GetProperty("refresh_token").GetString()!,
            ExpiresIn = root.TryGetProperty("expires_in", out var e) ? e.GetInt32() : 3600,
            RefreshExpiresIn = root.TryGetProperty("refresh_expires_in", out var re) ? re.GetInt32() : 3600
        };
    }
}