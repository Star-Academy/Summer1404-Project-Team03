using System.Text.Json.Serialization;

namespace etl_backend.Application.Dtos;

public class UserCreateDto: BaseUserDto
{
    
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    
}