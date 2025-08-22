namespace etl_backend.Configuration;

public class KeycloakOptions
{
    public string AuthServerUrl { get; set; } = string.Empty;
    public string Realm { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    public string AccessCookieName  { get; set; } = "kc_access";
    public string RefreshCookieName { get; set; } = "kc_refresh";

    public string ExpClaimType { get; set; } = "exp";
    public int ClockSkewSeconds { get; set; } = 30;
    
    public string Authority => $"{AuthServerUrl}/realms/{Realm}";
    public string ValidIssuer => $"{AuthServerUrl}/realms/{Realm}";
    public TimeSpan ClockSkew => TimeSpan.FromSeconds(ClockSkewSeconds);
}