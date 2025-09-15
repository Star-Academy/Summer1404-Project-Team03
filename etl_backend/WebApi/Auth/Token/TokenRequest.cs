namespace WebApi.Auth.Token;

public class TokenRequest
{
    public string Code { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
}