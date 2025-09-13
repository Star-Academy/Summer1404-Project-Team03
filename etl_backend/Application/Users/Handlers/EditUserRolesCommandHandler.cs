using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.Users.Commands;
using MediatR;

namespace Application.Users.Handlers;

public class EditUserRolesCommandHandler : IRequestHandler<EditUserRolesCommand>
{
    private readonly IUserManagementService _userManagementService;

    public EditUserRolesCommandHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task Handle(EditUserRolesCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        var currentUser = await _userManagementService.GetUserByIdAsync(request.UserId, ct);
        if (currentUser == null)
            throw new NotFoundException("User", request.UserId);

        if (request.RolesToAdd.Any())
        {
            var allRoles = await _userManagementService.GetAllRolesAsync(ct);
            var allRoleNames = allRoles.Select(r => r.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var invalidRoles = request.RolesToAdd
                .Where(r => !allRoleNames.Contains(r.Name))
                .Select(r => r.Name)
                .ToList();

            if (invalidRoles.Count > 0)
                throw new UnprocessableEntityException($"Invalid roles to add: {string.Join(", ", invalidRoles)}");
        }

        if (request.RolesToAdd.Any())
        {
            await _userManagementService.AddRolesToUserAsync(request.UserId, request.RolesToAdd.ToArray(), ct);
        }

        if (request.RolesToRemove.Any())
        {
            await _userManagementService.RemoveRolesFromUserAsync(request.UserId, request.RolesToRemove, ct);
        }
    }
}