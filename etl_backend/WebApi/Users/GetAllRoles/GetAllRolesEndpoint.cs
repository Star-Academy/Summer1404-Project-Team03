using Application.Dtos;
using Application.Users.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Users.GetAllRoles;

public class GetAllRolesEndpoint : EndpointWithoutRequest<GetAllRolesResponse>
{
    private readonly IMediator _mediator;

    public GetAllRolesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/admin/roles");
        Summary(s =>
        {
            s.Summary = "Get all available roles";
            s.Description = "Returns list of all roles in the system.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllRolesQuery(), ct);

        Response = new GetAllRolesResponse
        {
            Roles = result.Select(r => new RoleDto
            {
                Name = r.Name,
                Id =  r.Id,
            }).ToList()
        };
    }
}