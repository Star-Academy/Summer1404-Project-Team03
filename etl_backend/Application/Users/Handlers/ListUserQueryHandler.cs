using Application.Services.Abstractions;
using Application.Users.Queries;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Handlers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<UserWithRolesDto>>
{
    private readonly IUserManagementService _userManagementService;

    public ListUsersQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<List<UserWithRolesDto>> Handle(ListUsersQuery request, CancellationToken ct)
    {
        var users = await _userManagementService.GetAllUsersAsync(ct);
        return users.ToList();
    }
}