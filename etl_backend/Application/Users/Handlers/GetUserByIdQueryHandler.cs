using Application.Common.Authorization;
using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.Users.Queries;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserWithRolesDto>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserByIdQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<UserWithRolesDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        var user = await _userManagementService.GetUserByIdAsync(request.UserId, ct);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        return user;
    }
}