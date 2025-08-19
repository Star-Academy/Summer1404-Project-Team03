namespace etl_backend.DTO;

public class KeycloakTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public int RefreshExpiresIn { get; set; }  
}