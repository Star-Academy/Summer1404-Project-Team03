using System.Text.Json.Serialization;

namespace etl_backend.Application.UsersAuth.Dtos;

public class EditUserRolesRequestDto
{
    [JsonPropertyName("rolesToAdd")]
    public IEnumerable<RoleDto> RolesToAdd { get; set; } = Enumerable.Empty<RoleDto>();
    [JsonPropertyName("rolesToRemove")]
    public IEnumerable<RoleDto> RolesToRemove { get; set; } = Enumerable.Empty<RoleDto>();
}