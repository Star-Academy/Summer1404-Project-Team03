namespace Infrastructure.Identity.Abstractions;

public interface ITokenExpirationChecker
{
    bool IsAccessTokenExpired(string accessToken, TimeSpan clockSkew, string expClaimType);   
}