// using Application.Users.Commands;
// using FastEndpoints;
// using MediatR;
//
// namespace WebApi.Users.AssignRole;
//
// public class AssignRoleEndpoint : Endpoint<AssignRoleRequest>
// {
//     private readonly IMediator _mediator;
//
//     public AssignRoleEndpoint(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//
//     public override void Configure()
//     {
//         Post("api/admin/users/{UserId}/roles");
//         Summary(s =>
//         {
//             s.Summary = "Assign a role to a user";
//             s.Description = "Assigns a role to a user. Only sys_admin can perform this action.";
//             s.ExampleRequest = new AssignRoleRequest
//             {
//                 UserId = "user123",
//                 RoleName = "data_admin"
//             };
//         });
//     }
//
//     public override async Task HandleAsync(AssignRoleRequest req, CancellationToken ct)
//     {
//         await _mediator.Send(new AssignRoleToUserCommand(req.UserId, req.RoleName), ct);
//         await SendNoContentAsync(ct);
//     }
// }