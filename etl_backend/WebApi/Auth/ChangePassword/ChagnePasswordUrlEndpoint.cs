using FastEndpoints;
using Infrastructure.SsoServices.User.Abstractions;

namespace WebApi.Auth.ChangePassword;

public class ChangePasswordUrlEndpoint : EndpointWithoutRequest<ChangePasswordUrlResponse>
{
    private readonly IKeycloakAuthService _keycloakAuthService;

    public ChangePasswordUrlEndpoint(IKeycloakAuthService keycloakAuthService)
    {
        _keycloakAuthService = keycloakAuthService;
    }

    public override void Configure()
    {
        Get("api/auth/change-password-url");
        AllowAnonymous(); 
        Summary(s =>
        {
            s.Summary = "Get Keycloak change password URL";
            s.Description = "Returns URL to Keycloak's change password page.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var url = _keycloakAuthService.GenerateChangePasswordUrlPage();
        Response = new ChangePasswordUrlResponse { ChangePasswordUrl = url };
    }
}
