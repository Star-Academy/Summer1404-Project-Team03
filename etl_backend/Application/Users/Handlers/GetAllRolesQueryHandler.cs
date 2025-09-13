using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.Queries;
using MediatR;

namespace Application.Users.Handlers;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IUserManagementService _userManagementService;

    public GetAllRolesQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _userManagementService.GetAllRolesAsync(ct);
        return roles.ToList();
    }
}