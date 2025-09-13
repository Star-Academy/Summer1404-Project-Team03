using System.Text.Json.Serialization;

namespace Application.Dtos;

public class UserDto: BaseUserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } 
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    
    public UserDto() { }

    public UserDto(string username, string email, string? firstName = null, string? lastName = null)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
