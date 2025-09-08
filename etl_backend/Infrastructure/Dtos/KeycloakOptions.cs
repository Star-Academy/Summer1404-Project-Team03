namespace Infrastructure.Dtos;

public class KeycloakOptions
{
    public string AuthServerUrl { get; set; } = string.Empty;
    public string AuthServerUrlPublic { get; set; } =  string.Empty;
    public string Realm { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } =  string.Empty;
    public string Audience { get; set; } = string.Empty;

    public string AccessCookieName  { get; set; } = "access_token";
    public string RefreshCookieName { get; set; } = "refresh_token";

    public string ExpClaimType { get; set; } = "exp";
    public int ClockSkewSeconds { get; set; } = 30;
    
    public string RoleScope { get; set; } = "realm_access";
    public string RolesKey { get; set; } =  "roles";
    
    public string Authority => $"{AuthServerUrl}/realms/{Realm}";
    public string ValidIssuer => $"{AuthServerUrlPublic}/realms/{Realm}";
    public TimeSpan ClockSkew => TimeSpan.FromSeconds(ClockSkewSeconds);
}