using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.GetAllRoles.ServiceAbstractions;
using Application.Users.Queries;
using MediatR;

namespace Application.Users.Handlers;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IGetAllRoles _allRolesManager;

    public GetAllRolesQueryHandler(IGetAllRoles userRoleManagementService)
    {
        _allRolesManager = userRoleManagementService;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _allRolesManager.GetAllRolesAsync(ct);
        return roles.ToList();
    }
}