namespace etl_backend.Configuration;

public class KeycloakOptions
{
    public string AuthServerUrl { get; set; } = string.Empty;
    public string Realm { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}
