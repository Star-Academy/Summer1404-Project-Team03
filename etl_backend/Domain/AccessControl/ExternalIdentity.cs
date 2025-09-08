namespace Domain.AccessControl;

public sealed class ExternalIdentity
{
    public string UserId { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Email { get; init; } = default!;
    public IReadOnlyList<RoleName> Roles { get; init; } = Array.Empty<RoleName>();
}