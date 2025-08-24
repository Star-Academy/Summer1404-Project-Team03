using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace etl_backend.Api.Controllers;

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
