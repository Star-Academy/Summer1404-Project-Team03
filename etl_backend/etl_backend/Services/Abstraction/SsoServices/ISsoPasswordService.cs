namespace etl_backend.Services.Abstraction.SsoServices;


public interface ISsoPasswordService
{
    /// <summary>
    /// Lets a logged-in user change their own password via Keycloak.
    /// </summary>
    /// <param name="accessToken">The user’s own access token (Bearer).</param>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    Task ChangeOwnPasswordAsync(string accessToken, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
}
