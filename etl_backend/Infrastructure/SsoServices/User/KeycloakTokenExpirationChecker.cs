using Infrastructure.Identity.Abstractions;
using System.IdentityModel.Tokens.Jwt;


namespace Infrastructure.SsoServices.User;

public class KeycloakTokenExpirationChecker: ITokenExpirationChecker
{
    private readonly JwtSecurityTokenHandler _jwtHandler = new();
    
    public bool IsAccessTokenExpired(string accessToken, TimeSpan clockSkew, string expClaimType)
    {
        if (string.IsNullOrEmpty(accessToken)) return true;
        try
        {
            var jwt = _jwtHandler.ReadJwtToken(accessToken);
            var expUtc = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jwt.Claims.First(c => c.Type == expClaimType).Value));
            return DateTimeOffset.UtcNow - clockSkew >= expUtc;
        }
        catch
        {
            // treat parse errors as expired
            return true;
        }
    }
}