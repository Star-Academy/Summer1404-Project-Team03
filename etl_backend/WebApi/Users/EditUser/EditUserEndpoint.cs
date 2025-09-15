using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.EditUser;

public class EditUserEndpoint : Endpoint<EditUserRequest, EditUserResponse>
{
    private readonly IMediator _mediator;

    public EditUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("api/admin/users/{userId}");
        Summary(s =>
        {
            s.Summary = "Update user details";
            s.Description = "Updates user profile in Keycloak. Only sys_admin can perform this action.";
            // s.Params("UserId", "User ID to update", example: "user123");
            s.ExampleRequest = new EditUserRequest
            {
                FirstName = "John",
                LastName = "Updated"
            };
        });
    }

    public override async Task HandleAsync(EditUserRequest req, CancellationToken ct)
    {
        var userId = Route<string>("userId");

        var result = await _mediator.Send(new EditUserCommand(
            userId,
            req.Username,
            req.Email,
            req.FirstName,
            req.LastName
        ), ct);

        Response = new EditUserResponse
        {
            Id = result.Id,
            Username = result.Username,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName
        };
    }
}