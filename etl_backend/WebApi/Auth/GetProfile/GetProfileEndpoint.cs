using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.GetUserById.ServiceAbstractions;
using FastEndpoints;

namespace WebApi.Auth.GetProfile;

public class GetProfileEndpoint : EndpointWithoutRequest<GetProfileResponse>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IGetUserByIdService _getUserByIdService;

    public GetProfileEndpoint(ICurrentUserService currentUser, IGetUserByIdService getUserByIdService)
    {
        _currentUser = currentUser;
        _getUserByIdService = getUserByIdService;
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
        var UserDto = await _getUserByIdService.GetUserByIdAsync(_currentUser.UserId, ct); 
        Response = new GetProfileResponse
        {
            Id = _currentUser.UserId ?? "unknown",
            Name = _currentUser.UserName ?? "Unknown User",
            Roles = new List<RoleDto>(UserDto.Roles),
            FirstName = UserDto.FirstName?.Trim() ?? string.Empty,
            LastName = UserDto.LastName?.Trim() ?? string.Empty, 
            Email = UserDto.Email
        };
    }
}