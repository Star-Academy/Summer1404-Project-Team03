namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenExpirationChecker
{
    bool IsAccessTokenExpired(string accessToken, TimeSpan clockSkew, string expClaimType);   
}