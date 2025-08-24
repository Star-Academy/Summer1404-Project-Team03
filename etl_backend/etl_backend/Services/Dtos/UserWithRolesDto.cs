using System.Text.Json.Serialization;

namespace etl_backend.Services.Dtos;

public class UserWithRolesDto: UserDto
{
    [JsonPropertyName("roles")]
    public IEnumerable<RoleDto> Roles { get; set; } = Enumerable.Empty<RoleDto>();

    public UserWithRolesDto() { }

    public UserWithRolesDto(UserDto user, IEnumerable<RoleDto> roles)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Roles = roles;
    }
}
