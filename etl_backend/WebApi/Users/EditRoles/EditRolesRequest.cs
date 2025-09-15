using System.Text.Json.Serialization;
using Application.Dtos;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Users.EditRoles;

public class EditUserRolesRequest
{
    [JsonPropertyName("rolesToAdd")]
    public IEnumerable<RoleDto> RolesToAdd { get; set; } = Enumerable.Empty<RoleDto>();
    [JsonPropertyName("rolesToRemove")]
    public IEnumerable<RoleDto> RolesToRemove { get; set; } = Enumerable.Empty<RoleDto>();
}