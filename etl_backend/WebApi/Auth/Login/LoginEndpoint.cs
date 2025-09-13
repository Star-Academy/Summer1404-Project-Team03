using FastEndpoints;
using Infrastructure.SsoServices.User.Abstractions;

namespace WebApi.Auth.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IKeycloakAuthService _keycloakAuthService;

    public LoginEndpoint(IKeycloakAuthService keycloakAuthService)
    {
        _keycloakAuthService = keycloakAuthService;
    }

    public override void Configure()
    {
        Post("api/auth/login");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Initiate Keycloak login";
            s.Description = "Generates Keycloak login URL. Redirect frontend to this URL.";
            s.ExampleRequest = new LoginRequest { RedirectUrl = "https://localhost:4200/token-code" };
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var url = _keycloakAuthService.GenerateLoginUrl(req.RedirectUrl);
        Response = new LoginResponse { RedirectUrl = url };
    }
}