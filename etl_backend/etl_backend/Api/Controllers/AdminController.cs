using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace etl_backend.Api.Controllers;

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
    private readonly IEditUserRolesService _editUserRolesService;
    private readonly IGetRolesList _getRolesList;

    public AdminController(
        IGetAllUsersService getAllUsersService,
        IGetUserByIdService getUserByIdService,
        ICreateUserService createUserService,
        IEditUserService editUserService,
        IDeleteUserService deleteUserService, 
        IEditUserRolesService editUserRolesService, 
        IGetRolesList getRolesList)
    {
        _getAllUsersService = getAllUsersService;
        _getUserByIdService = getUserByIdService;
        _createUserService = createUserService;
        _editUserService = editUserService;
        _deleteUserService = deleteUserService;
        _editUserRolesService = editUserRolesService;
        _getRolesList = getRolesList;
    }
    
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _getAllUsersService.ExecuteAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<UserWithRolesDto>> GetUserById(string userId, CancellationToken cancellationToken)
    {
        
        var user = await _getUserByIdService.ExecuteAsync(userId, cancellationToken);
        return Ok(user);
    }

    [HttpPost("users")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto createDto, CancellationToken cancellationToken)
    {
        var createdUser = await _createUserService.ExecuteAsync(createDto, cancellationToken);
        return createdUser;
    }

    [HttpPut("users/{userId}")]
    public async Task<IActionResult> EditUser(string userId, [FromBody] EditUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        
        await _editUserService.ExecuteAsync(userId, requestDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        await _deleteUserService.ExecuteAsync(userId, cancellationToken);
        return NoContent();
    }
    
    [HttpPut("users/{userId}/roles")]
    public async Task<ActionResult<UserWithRolesDto>> EditUserRoles(
        string userId,
        [FromBody] EditUserRolesRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var updatedUser = await _editUserRolesService.ExecuteAsync(
            userId,
            requestDto.RolesToAdd,
            requestDto.RolesToRemove,
            cancellationToken);

        return Ok(updatedUser);
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles(CancellationToken cancellationToken)
    {
        var allRoles = await _getRolesList.ExecuteAsync(cancellationToken);
        return Ok(allRoles);
    }
}

