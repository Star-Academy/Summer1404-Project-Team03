using Application.Users.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.ListUsers;

public class ListUsersEndpoint : EndpointWithoutRequest<ListUsersResponse>
{
    private readonly IMediator _mediator;

    public ListUsersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/users");
        // ✅ Pipeline enforces sys_admin role via [RequireRole]
        Summary(s =>
        {
            s.Summary = "List all users";
            s.Description = "Returns list of all users and their roles. Only sys_admin can access.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new ListUsersQuery(), ct);

        Response = new ListUsersResponse
        {
            Users = result.Select(u => new UserItem
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Roles = u.Roles.Select(r => r.Name).ToList()
            }).ToList()
        };
    }
}