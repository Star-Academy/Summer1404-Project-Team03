using Application.Services.Abstractions;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Auth.UpdateProfile;

public class UpdateProfileEndpoint : Endpoint<UpdateProfileRequest>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public UpdateProfileEndpoint(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    public override void Configure()
    {
        Put("api/auth/me");
        Summary(s =>
        {
            s.Summary = "Update current user profile";
            s.Description = "Updates profile of the authenticated user.";
            s.ExampleRequest = new UpdateProfileRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
        });
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var userId = _currentUser.UserId;
        if (userId == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await _mediator.Send(new EditUserCommand(
            userId,
            null, 
            req.Email,
            req.FirstName,
            req.LastName
        ), ct);

        await SendNoContentAsync(ct);
    }
}