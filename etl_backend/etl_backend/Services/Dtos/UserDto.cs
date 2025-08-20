namespace etl_backend.Services;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; } // only used when creating/changing password

    public UserDto() { }

    public UserDto(string username, string email, string? firstName = null, string? lastName = null, bool emailVerified = false)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        EmailVerified = emailVerified;
    }
}
