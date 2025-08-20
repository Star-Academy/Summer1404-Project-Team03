using etl_backend.Services;
using etl_backend.Services.Abstraction;

namespace etl_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ISsoProfileService _profileService;

    public ProfileController(ISsoProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserProfile>> GetProfile(CancellationToken cancellationToken)
    {
        var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized("Missing or invalid Authorization header");
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var profile = await _profileService.GetProfileAsync(token, cancellationToken);
        return Ok(profile);
    }
}
