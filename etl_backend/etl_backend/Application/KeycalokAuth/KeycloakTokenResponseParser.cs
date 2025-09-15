using System.Text.Json;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.KeycalokAuth;

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