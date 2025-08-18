using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = etl_backend.Services.IAuthorizationService;

namespace etl_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TempCheckClaimsController : ControllerBase
{
    // GET
    private readonly IAuthorizationService _authService;

    public TempCheckClaimsController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    [HttpGet("admin-data")]
    
    [Authorize(Policy = "RequireDataAdmin")]
    public IActionResult GetAdminData()
    {
        // if (!_authService.HasRole(User, "data_admin"))
        //     return Forbid();
        //
        // return Ok("This is admin-only data.");
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}