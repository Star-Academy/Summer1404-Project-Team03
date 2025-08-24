namespace etl_backend.Services.Auth.keycloakService.Dtos;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public int ExpiresIn { get; set; }          
    public int RefreshExpiresIn { get; set; }   
}