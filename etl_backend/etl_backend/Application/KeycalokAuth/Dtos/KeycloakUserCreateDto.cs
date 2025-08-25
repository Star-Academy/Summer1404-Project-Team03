using System.Text.Json.Serialization;

namespace etl_backend.Application.KeycalokAuth.Dtos;

public class KeycloakUserCreateDto
{
    [JsonPropertyName("username")] public string Username { get; set; } = null!;

    [JsonPropertyName("firstName")] public string? FirstName { get; set; }

    [JsonPropertyName("lastName")] public string? LastName { get; set; }

    [JsonPropertyName("email")] public string? Email { get; set; }

    [JsonPropertyName("enabled")] public bool Enabled { get; set; } = true;


    // Optional: initial password
    [JsonPropertyName("credentials")] public List<KeycloakCredentialDto>? Credentials { get; set; }

    // Optional: custom attributes
    [JsonPropertyName("attributes")] public Dictionary<string, string>? Attributes { get; set; }
}

public class KeycloakCredentialDto
{
    [JsonPropertyName("type")] 
    public string Type { get; set; } = "password";

    [JsonPropertyName("value")] 
    public string Value { get; set; } = null!;

    [JsonPropertyName("temporary")] 
    public bool Temporary { get; set; } = true;
}