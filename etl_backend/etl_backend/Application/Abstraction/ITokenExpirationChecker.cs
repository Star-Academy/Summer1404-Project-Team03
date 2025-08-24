namespace etl_backend.Application.Abstraction;

public interface ITokenExpirationChecker
{
    bool IsAccessTokenExpired(string accessToken, TimeSpan clockSkew, string expClaimType);   
}