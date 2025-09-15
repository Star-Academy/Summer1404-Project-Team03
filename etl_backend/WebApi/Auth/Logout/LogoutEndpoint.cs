using Application.Services.Abstractions;
using FastEndpoints;
using Infrastructure.Identity.Abstractions;
using Infrastructure.SsoServices.User.Abstractions;

namespace WebApi.Auth.Logout;

public class LogoutEndpoint : EndpointWithoutRequest
{
    
    private readonly IKeycloakLogOutUser _keycloakLogOutUser;
    private readonly ITokenCookieService _tokenCookieService;
    private readonly ICurrentUserService _currentUser;

    public LogoutEndpoint(
        IKeycloakLogOutUser keycloakLogOutUser,
        ITokenCookieService tokenCookieService,
        ICurrentUserService currentUser)
    {
        _keycloakLogOutUser = keycloakLogOutUser;
        _tokenCookieService = tokenCookieService;
        _currentUser = currentUser;
    }

    public override void Configure()
    {
        Post("api/auth/logout");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Logout current user";
            s.Description = "Revokes refresh token (if supported) and clears authentication cookies.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var userId = _currentUser.UserId;
        if (userId == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var logoutSuccess = await _keycloakLogOutUser.LogOutAsynk(userId, ct);

        _tokenCookieService.ClearTokens(HttpContext.Response);

        if (logoutSuccess)
        {
            await SendNoContentAsync(ct);
        }
        else
        {
            await SendNoContentAsync(ct);
        }
    }
}