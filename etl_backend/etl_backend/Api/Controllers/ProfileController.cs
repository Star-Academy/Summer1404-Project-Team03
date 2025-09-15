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
    private readonly IEditUserService _editUserService;
    
    public ProfileController(ITokenProfileExtractor tokenProfileExtractor, IEditUserService editUserService)
    {
        _tokenProfileExtractor = tokenProfileExtractor;
        _editUserService = editUserService;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserWithRolesDto>> GetProfile(CancellationToken cancellationToken)
    {

        var profile = await _tokenProfileExtractor.ExtractProfile(User);
        return Ok(profile);
        
    }
    
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] EditUserRequestDto profile, CancellationToken cancellationToken)
    {
        var currentUser = await _tokenProfileExtractor.ExtractProfile(User);
        var userId = currentUser.Id;
        await _editUserService.ExecuteAsync(userId, profile, cancellationToken);
        return NoContent();
    }
}
