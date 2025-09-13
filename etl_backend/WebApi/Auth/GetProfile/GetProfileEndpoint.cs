using Application.Services.Abstractions;
using Application.ValueObjects;
using FastEndpoints;

namespace WebApi.Auth.GetProfile;

public class GetProfileEndpoint : EndpointWithoutRequest<GetProfileResponse>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserManagementService _userManagementService;

    public GetProfileEndpoint(ICurrentUserService currentUser, IUserManagementService userManagementService)
    {
        _currentUser = currentUser;
        _userManagementService = userManagementService;
    }

    public override void Configure()
    {
        Get("api/auth/me");
        Summary(s =>
        {
            s.Summary = "Get current user profile";
            s.Description = "Returns ID, name, and roles of the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        var UserDto = await _userManagementService.GetUserByIdAsync(_currentUser.UserId, ct); 
        Response = new GetProfileResponse
        {
            Id = _currentUser.UserId ?? "unknown",
            Name = _currentUser.UserName ?? "Unknown User",
            Roles = (List<RoleDto>)UserDto.Roles,
            FirstName = UserDto.FirstName?.Trim() ?? string.Empty,
            LastName = UserDto.LastName?.Trim() ?? string.Empty, 
            Email = UserDto.Email
        };
    }
}