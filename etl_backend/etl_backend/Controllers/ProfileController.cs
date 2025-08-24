using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace etl_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/users")]
public class ProfileController : ControllerBase
{
    
    private readonly ITokenProfileExtractor _tokenProfileExtractor;
    public ProfileController(ITokenProfileExtractor tokenProfileExtractor)
    {
        _tokenProfileExtractor = tokenProfileExtractor;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserWithRolesDto>> GetProfile(CancellationToken cancellationToken)
    {

        var profile = await _tokenProfileExtractor.ExtractProfile(User);
        return Ok(profile);
        
    }
}
