using System.Text.Json;
using etl_backend.Configuration;
using etl_backend.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace etl_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly KeycloakOptions _options;
    private readonly HttpClient _httpClient;

    public AuthController(IOptions<KeycloakOptions> options, IHttpClientFactory httpClientFactory)
    {
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestBodyDto request)
    {
        var url =
            $"{_options.AuthServerUrl}/realms/{_options.Realm}/protocol/openid-connect/auth" +
            $"?client_id={Uri.EscapeDataString(_options.ClientId)}" +
            $"&redirect_uri={Uri.EscapeDataString(request.RedirectUrl)}" +
            $"&response_type=code" +
            $"&scope=openid";

        return Ok(new { redirectUrl = url });
    }

    [HttpPost("token")]
    public async Task<IActionResult> SetToken([FromBody] SetTokenRequestDto request)
    {
        var tokenEndpoint = $"{_options.AuthServerUrl}/realms/{_options.Realm}/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _options.ClientId,
            ["code"] = request.Code,
            ["redirect_uri"] = request.RedirectUrl
        };
        
        

        var response = await _httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(formData));
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, content);
        }

        var json = JsonDocument.Parse(content).RootElement;

        var accessToken = json.GetProperty("access_token").GetString();
        var refreshToken = json.GetProperty("refresh_token").GetString();
        int accessExpiresIn = json.TryGetProperty("expires_in", out var expEl) ? expEl.GetInt32() : 3600;
        int refreshExpiresIn = json.TryGetProperty("refresh_expires_in", out var rExpEl) ? rExpEl.GetInt32() : 3600;

        SetTokenCookies(accessToken!, refreshToken!, accessExpiresIn, refreshExpiresIn);

        return Ok(new { message = "Tokens set successfully" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        RemoveTokenCookies();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize(Policy = "RequireAnalyst")]
    public IActionResult GetUserInfo()
    {
        var claims = User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToList();
        return Ok(claims);
    }

    // -----------------------
    // Helpers
    // -----------------------

    private void SetTokenCookies(string accessToken, string refreshToken, int accessExpiresIn, int refreshExpiresIn)
    {
        Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddSeconds(accessExpiresIn)
        });

        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddSeconds(refreshExpiresIn)
        });
    }

    private void RemoveTokenCookies()
    {
        
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");
    }
}
