using Application.Common.Exceptions;
using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.Commands;
using MediatR;

namespace Application.Users.Handlers;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand>
{
    private readonly IUserManagementService _userManagementService;

    public AssignRoleToUserCommandHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task Handle(AssignRoleToUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        if (string.IsNullOrWhiteSpace(request.RoleName))
            throw new UnprocessableEntityException("RoleName is required.");

        var user = await _userManagementService.GetUserByIdAsync(request.UserId, ct);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        var allRoles = await _userManagementService.GetAllRolesAsync(ct);
        var roleExists = allRoles.Any(r => string.Equals(r.Name, request.RoleName, StringComparison.OrdinalIgnoreCase));
        if (!roleExists)
            throw new UnprocessableEntityException($"Role '{request.RoleName}' does not exist.");

        var userHasRole = user.Roles.Any(r => string.Equals(r.Name, request.RoleName, StringComparison.OrdinalIgnoreCase));
        if (userHasRole)
        {
            return;
        }

        await _userManagementService.AddRolesToUserAsync(
            request.UserId,
            new[] { new RoleDto { Name = request.RoleName } },
            ct
        );
    }
}