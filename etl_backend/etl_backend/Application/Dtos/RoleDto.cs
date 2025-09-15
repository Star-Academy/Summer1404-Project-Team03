using System.Text.Json.Serialization;

namespace etl_backend.Application.Dtos;

public class RoleDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
}