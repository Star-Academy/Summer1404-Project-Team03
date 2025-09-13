namespace WebApi.Users.ListUsers;

public class ListUsersResponse
{
    public List<UserItem> Users { get; set; } = new();
}

public class UserItem
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<string> Roles { get; set; } = new();
}