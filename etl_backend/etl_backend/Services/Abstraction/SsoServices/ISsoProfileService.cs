using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.SsoServices;

public interface ISsoProfileService
{
    /// <summary>
    /// Returns the current user's profile using the provided access token.
    /// </summary>
    /// <param name="accessToken">Raw access token (without the "Bearer " prefix).</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<UserProfile> GetProfileAsync(string accessToken, CancellationToken cancellationToken = default);
}
