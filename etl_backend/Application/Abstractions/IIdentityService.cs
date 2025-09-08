using Application.ValueObjects;

namespace Application.Abstractions;

public interface IIdentityService
{
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken ct);
    Task LogoutAsync(string userId, CancellationToken ct);
    Task<ProfileDto> GetProfileAsync(string userId, CancellationToken ct);
    Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct);
}