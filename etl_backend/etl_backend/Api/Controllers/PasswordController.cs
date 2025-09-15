using etl_backend.Application.KeycalokAuth.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace etl_backend.Api.Controllers;

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
            changePasswordUrl = _keycloakAuthService.GenerateChangePasswordUrlPage()
        });
    }
}

