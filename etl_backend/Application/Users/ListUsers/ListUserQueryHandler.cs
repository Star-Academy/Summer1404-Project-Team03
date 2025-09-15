using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.ListUsers;
using Application.Users.Queries;
using MediatR;

namespace Application.Users.Handlers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<UserWithRolesDto>>
{
    private readonly IListUsersService _userManagementService;

    public ListUsersQueryHandler(IListUsersService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<List<UserWithRolesDto>> Handle(ListUsersQuery request, CancellationToken ct)
    {
        var users = await _userManagementService.GetAllUsersAsync(ct);
        return users.ToList();
    }
}