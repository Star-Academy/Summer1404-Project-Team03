using Application.Dtos;

namespace WebApi.Users.GetAllRoles;

public class GetAllRolesResponse
{
    public List<RoleDto> Roles { get; set; } = new();
}