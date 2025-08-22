using System.Text.Json;
using etl_backend.Configuration;
using etl_backend.DTO;
using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Auth.keycloakService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace etl_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly KeycloakOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ITokenCookieService _tokenCookieService;
    private readonly ITokenExtractor _tokenExtractor;
    private readonly IKeycloakAuthService  _keycloakAuthService;
    private readonly IKeycloakRefreshTokenRevokable _keycloakRefreshRevoker;

    public AuthController(IOptions<KeycloakOptions> options, IHttpClientFactory httpClientFactory, ITokenCookieService tokenCookieService, ITokenExtractor tokenExtractor, IKeycloakAuthService keycloakAuthService, IKeycloakRefreshTokenRevokable keycloakRefreshRevoker)
    {
        _tokenCookieService = tokenCookieService;
        _tokenExtractor = tokenExtractor;
        _keycloakAuthService = keycloakAuthService;
        _keycloakRefreshRevoker = keycloakRefreshRevoker;
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestBodyDto request)
    {

        var url = _keycloakAuthService.GenerateLoginUrl(request.RedirectUrl);

        return Ok(new { redirectUrl = url });
    }

    [HttpPost("token")]
    public async Task<IActionResult> SetToken([FromBody] SetTokenRequestDto request)
    {
        
        var tokens = await _keycloakAuthService.ExchangeCodeForTokensAsync(request.Code, request.RedirectUrl);
        
        _tokenCookieService.SetTokens(Response, tokens);

        return Ok(new { message = "Tokens set successfully" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
         
        var okResponse = Ok(new { message = "Logged out successfully" });
        var refreshToken = _tokenExtractor.GetRefreshToken(Request, _options.RefreshCookieName);
        if (refreshToken.IsNullOrEmpty()) return okResponse;
        
        var revokedSuccessfully = await _keycloakRefreshRevoker.RevokeTokenAsynk(refreshToken!);
        // for now no need to check revoking was successfully of not.
        _tokenCookieService.RemoveTokens(Response);
        return okResponse;
    }

    [HttpGet("me")]
    [Authorize(Policy = "RequireSysAdmin")]
    public IActionResult GetUserInfo()
    {
        var claims = User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToList();
        return Ok(claims);
    }
    
}
