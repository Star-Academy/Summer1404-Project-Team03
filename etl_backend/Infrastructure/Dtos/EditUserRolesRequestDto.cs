using System.Text.Json.Serialization;

namespace Infrastructure.Dtos;

public class EditUserRolesRequestDto
{
    [JsonPropertyName("rolesToAdd")]
    public IEnumerable<RoleDto> RolesToAdd { get; set; } = Enumerable.Empty<RoleDto>();
    [JsonPropertyName("rolesToRemove")]
    public IEnumerable<RoleDto> RolesToRemove { get; set; } = Enumerable.Empty<RoleDto>();
}