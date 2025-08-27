namespace etl_backend.Api.Dtos;

public class SetTokenRequestDto
{
    public string Code { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
}