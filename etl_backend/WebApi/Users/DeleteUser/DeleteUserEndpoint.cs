using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.DeleteUser;

public class DeleteUserEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public DeleteUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/admin/users/{userId}");
        Summary(s =>
        {
            s.Summary = "Delete a user";
            s.Description = "Deletes a user from Keycloak. Only sys_admin can perform this action.";
            // s.Params("UserId", "User ID to delete", example: "user123");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<string>("userId");
        await _mediator.Send(new DeleteUserCommand(userId), ct);
        await SendNoContentAsync(ct);
    }
}