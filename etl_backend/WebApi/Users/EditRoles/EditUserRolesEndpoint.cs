using Application.Users.Commands;
using Application.ValueObjects;
using FastEndpoints;
using Infrastructure.Dtos;
using MediatR;

namespace WebApi.Users.EditRoles;

public class EditUserRolesEndpoint : Endpoint<EditUserRolesRequest>
{
    private readonly IMediator _mediator;

    public EditUserRolesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("api/admin/users/{userId}/roles");
        Summary(s =>
        {
            s.Summary = "Edit user roles";
            s.Description = "Adds or removes roles for a user. Only sys_admin can perform this action.";
            // s.Params("UserId", "User ID", example: "user123");
            s.ExampleRequest = new EditUserRolesRequestDto
            {
                RolesToAdd = new List<RoleDto> { new() { Name = "analyst" } },
                RolesToRemove = new List<RoleDto> { new() { Name = "data_admin" } }
            };
        });
    }

    public override async Task HandleAsync(EditUserRolesRequest req, CancellationToken ct)
    {
        var userId = Route<string>("userId");
        Console.WriteLine((userId ?? "\n"));
        await _mediator.Send(new EditUserRolesCommand(
            userId,
            req.RolesToAdd?.ToList() ?? new List<RoleDto>(),
            req.RolesToRemove?.ToList() ?? new List<RoleDto>()
        ), ct);

        await SendNoContentAsync(ct);
    }
}