using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.SsoServices;

namespace etl_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly ISsoPasswordService _passwordService;

    public PasswordController(ISsoPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    [HttpPost("change")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        // Get token from Authorization header
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
            return Unauthorized("Missing or invalid Authorization header");

        var token = authHeader["Bearer ".Length..];

        await _passwordService.ChangeOwnPasswordAsync(token, request.CurrentPassword, request.NewPassword, cancellationToken);

        return NoContent();
    }
}

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
