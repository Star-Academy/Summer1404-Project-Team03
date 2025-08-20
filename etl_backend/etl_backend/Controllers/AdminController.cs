using etl_backend.Services;
using etl_backend.Services.Abstraction.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace etl_backend.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "RequireSysAdmin")]
public class AdminController : ControllerBase
{
    private readonly IGetAllUsersService _getAllUsersService;
    private readonly IGetUserByIdService _getUserByIdService;
    private readonly ICreateUserService _createUserService;
    private readonly IEditUserService _editUserService;
    private readonly IDeleteUserService _deleteUserService;

    public AdminController(
        IGetAllUsersService getAllUsersService,
        IGetUserByIdService getUserByIdService,
        ICreateUserService createUserService,
        IEditUserService editUserService,
        IDeleteUserService deleteUserService)
    {
        _getAllUsersService = getAllUsersService;
        _getUserByIdService = getUserByIdService;
        _createUserService = createUserService;
        _editUserService = editUserService;
        _deleteUserService = deleteUserService;
    }

    private string GetAccessToken()
    {
        if (HttpContext.Request.Cookies.TryGetValue("access_token", out var token))
        {
            return token;
        }

        throw new UnauthorizedAccessException("Access token not found in cookies.");
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        var users = await _getAllUsersService.ExecuteAsync(token, cancellationToken);
        return Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(string userId, CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        var user = await _getUserByIdService.ExecuteAsync(userId, token, cancellationToken);
        return Ok(user);
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        await _createUserService.ExecuteAsync(requestDto.User, requestDto.Roles, token, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new { userId = requestDto.User.Id }, null);
    }

    [HttpPut("users/{userId}")]
    public async Task<IActionResult> EditUser(string userId, [FromBody] EditUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        await _editUserService.ExecuteAsync(userId, requestDto.User, requestDto.Roles, token, cancellationToken);
        return NoContent();
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        await _deleteUserService.ExecuteAsync(userId, token, cancellationToken);
        return NoContent();
    }
}

