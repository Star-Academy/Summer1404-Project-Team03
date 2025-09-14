using Infrastructure.Dtos;

namespace Infrastructure.SsoServices.Admin.Mappers;
using Application.Dtos;
using System.Text.Json;

internal static class UserMapper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = false
    };

    public static JsonElement ToJsonElement(EditUserRequestDto dto)
    {
        string json = JsonSerializer.Serialize(dto, _jsonOptions);
        return JsonSerializer.Deserialize<JsonElement>(json, _jsonOptions);
    }
    public static UserDto FromJsonElement(JsonElement element)
    {
        string? GetProp(string name) =>
            element.TryGetProperty(name, out var prop) ? prop.GetString() : null;

        return new UserDto
        {
            Id = GetProp("id") ?? string.Empty,
            Username = GetProp("username") ?? string.Empty,
            Email = GetProp("email") ?? string.Empty,
            FirstName = GetProp("firstName"),
            LastName = GetProp("lastName")
        };
    }

    public static UserWithRolesDto ToUserWithRolesDto(UserDto user, IEnumerable<RoleDto> roles)
        => new(user, roles);

    public static UserCreateDto ToUserCreateDto(KeycloakUserCreateDto keycloakDto)
        => new()
        {
            Username = keycloakDto.Username,
            Email = keycloakDto.Email,
            FirstName = keycloakDto.FirstName,
            LastName = keycloakDto.LastName,
            Password = keycloakDto.Credentials?.FirstOrDefault()?.Value
        };

    public static KeycloakUserCreateDto ToKeycloakUserCreateDto(UserCreateDto appDto)
        => new()
        {
            Username = appDto.Username,
            Email = appDto.Email,
            FirstName = appDto.FirstName,
            LastName = appDto.LastName,
            Credentials = new List<KeycloakCredentialDto>
            {
                new()
                {
                    Temporary = true,
                    Type = "password",
                    Value = appDto.Password ?? appDto.Username
                }
            }
        };
}