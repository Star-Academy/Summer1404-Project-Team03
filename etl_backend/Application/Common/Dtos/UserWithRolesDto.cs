using System.Text.Json.Serialization;

namespace Application.Dtos;

public class UserWithRolesDto: UserDto
{
    public IEnumerable<RoleDto> Roles { get; set; } = Enumerable.Empty<RoleDto>();

    public UserWithRolesDto() { }

    public UserWithRolesDto(UserDto user, IEnumerable<RoleDto> roles)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Roles = roles;
    }
    public UserDto ToUserDto() => this;
}
