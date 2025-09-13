using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.CreateUser;

public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/admin/users");
        Summary(s =>
        {
            s.Summary = "Create a new user";
            s.Description = "Creates a new user in Keycloak. Only sys_admin can perform this action.";
            s.ExampleRequest = new CreateUserRequest
            {
                Username = "johndoe",
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "TempPass123!"
            };
        });
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateUserCommand(
            req.Username,
            req.Email,
            req.FirstName,
            req.LastName,
            req.Password
        ), ct);

        Response = new CreateUserResponse
        {
            Id = result.Id,
            Username = result.Username,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName
        };
    }
}