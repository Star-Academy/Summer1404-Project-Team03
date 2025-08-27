using etl_backend.Api.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace etl_backend.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    
    private readonly ITokenCookieService _tokenCookieService;
    private readonly IKeycloakAuthService  _keycloakAuthService;
    private readonly IKeycloakLogOutUser _keycloakLogOutUser;
    private readonly ITokenProfileExtractor _tokenProfileExtractor;

    public AuthController(ITokenCookieService tokenCookieService, IKeycloakAuthService keycloakAuthService, IKeycloakLogOutUser keycloakLogOutUser, ITokenProfileExtractor tokenProfileExtractor)
    {
        _tokenCookieService = tokenCookieService;
        _keycloakAuthService = keycloakAuthService;
        _keycloakLogOutUser = keycloakLogOutUser;
        _tokenProfileExtractor = tokenProfileExtractor;
        
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

        var currentUser = await _tokenProfileExtractor.ExtractProfile(User);
        var userId = currentUser.Id;
        var logoutSuccessfully = await _keycloakLogOutUser.LogOutAsynk(userId, CancellationToken.None);
        return logoutSuccessfully ? NoContent() : BadRequest();
        
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
