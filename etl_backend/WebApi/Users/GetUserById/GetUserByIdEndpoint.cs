using Application.Users.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.GetUserById;

public class GetUserEndpoint : EndpointWithoutRequest<GetUserResponse>
{
    private readonly IMediator _mediator;

    public GetUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/admin/users/{userId}");
        Summary(s =>
        {
            s.Summary = "Get user by ID";
            s.Description = "Returns user details and roles. Only sys_admin can access.";
            // s.Params("UserId", "User ID", example: "user123");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<string>("userId");
        Console.WriteLine(userId);
        var result = await _mediator.Send(new GetUserByIdQuery(userId), ct);

        Response = new GetUserResponse
        {
            Id = result.Id,
            Username = result.Username,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Roles = result.Roles.Select(r => r.Name).ToList()
        };
    }
}