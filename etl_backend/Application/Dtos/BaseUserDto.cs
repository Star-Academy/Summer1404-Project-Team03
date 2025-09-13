using System.Text.Json.Serialization;

namespace Application.Dtos;

public class BaseUserDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; } 
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }    
}