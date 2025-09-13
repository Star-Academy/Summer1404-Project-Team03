using FastEndpoints;
using Infrastructure.Identity.Abstractions;
using Infrastructure.SsoServices.User.Abstractions;

namespace WebApi.Auth.Token;

public class TokenEndpoint : Endpoint<TokenRequest, TokenResponse>
{
    private readonly IKeycloakAuthService _keycloakAuthService;
    private readonly ITokenCookieService _tokenCookieService;

    public TokenEndpoint(
        IKeycloakAuthService keycloakAuthService,
        ITokenCookieService tokenCookieService)
    {
        _keycloakAuthService = keycloakAuthService;
        _tokenCookieService = tokenCookieService;
    }

    public override void Configure()
    {
        Post("api/auth/token");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Exchange authorization code for tokens";
            s.Description = "Call after Keycloak redirects back with code. Sets tokens as cookies.";
            s.ExampleRequest = new TokenRequest
            {
                Code = "abc123...",
                RedirectUrl = "https://localhost:4200/"
            };
        });
    }

    public override async Task HandleAsync(TokenRequest req, CancellationToken ct)
    {
        var tokens = await _keycloakAuthService.ExchangeCodeForTokensAsync(req.Code, req.RedirectUrl, ct);
        _tokenCookieService.SetTokens(HttpContext.Response, tokens);

        Response = new TokenResponse();
    }
}