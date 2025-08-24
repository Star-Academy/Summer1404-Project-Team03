using etl_backend.Services.Auth.keycloakService.Abstraction;

namespace etl_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/password")]
public class PasswordController : ControllerBase
{
    private readonly IKeycloakAuthService _keycloakAuthService;

    public PasswordController(IKeycloakAuthService keycloakAuthService)
    {
        _keycloakAuthService = keycloakAuthService;
        
    }

    [HttpGet("change-password-url")]
    public IActionResult ChangePassword()
    {
        return Ok(new {
            changePasswordUrl = _keycloakAuthService.GenerateChangePasswordUrl()
        });
    }
}

