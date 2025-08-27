namespace etl_backend.Application.UsersAuth.Abstraction;

public interface ITokenExpirationChecker
{
    bool IsAccessTokenExpired(string accessToken, TimeSpan clockSkew, string expClaimType);   
}