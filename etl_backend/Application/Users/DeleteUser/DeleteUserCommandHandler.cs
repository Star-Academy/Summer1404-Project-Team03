using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.Users.Commands;
using MediatR;

namespace Application.Users.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserManagementService _userManagementService;

    public DeleteUserCommandHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        var existingUser = await _userManagementService.GetUserByIdAsync(request.UserId, ct);
        if (existingUser == null)
            throw new NotFoundException("User", request.UserId);

        await _userManagementService.DeleteUserAsync(request.UserId, ct);
    }
}