namespace Application.Services.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string[] Roles { get; }
    bool IsAuthenticated { get; }
}