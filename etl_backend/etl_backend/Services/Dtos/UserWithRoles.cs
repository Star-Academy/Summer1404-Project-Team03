namespace etl_backend.Services;

public class UserWithRolesDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

    public UserWithRolesDto() { }

    public UserWithRolesDto(UserDto user, IEnumerable<string> roles)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        EmailVerified = user.EmailVerified;
        Roles = roles;
    }
}
