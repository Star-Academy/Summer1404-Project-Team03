using Application.Services.Abstractions;
using FastEndpoints;

namespace WebApi.Auth.GetProfile;

public class GetProfileEndpoint : EndpointWithoutRequest<GetProfileResponse>
{
    private readonly ICurrentUserService _currentUser;

    public GetProfileEndpoint(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
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

        Response = new GetProfileResponse
        {
            Id = _currentUser.UserId ?? "unknown",
            Name = _currentUser.UserName ?? "Unknown User",
            Roles = _currentUser.Roles.ToList()
        };
    }
}