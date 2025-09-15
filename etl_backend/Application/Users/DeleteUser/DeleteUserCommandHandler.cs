using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.Users.Commands;
using Application.Users.DeleteUser.ServiceAbstractions;
using Application.Users.GetUserById.ServiceAbstractions;
using MediatR;

namespace Application.Users.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IDeleteUserService _deleteUserService;
    private readonly IGetUserByIdService _getUserByIdService;

    public DeleteUserCommandHandler(IDeleteUserService deleteUserService, IGetUserByIdService getUserByIdService)
    {
        _deleteUserService = deleteUserService;
        _getUserByIdService = getUserByIdService;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        var existingUser = await _getUserByIdService.GetUserByIdAsync(request.UserId, ct);
        if (existingUser == null)
            throw new NotFoundException("User", request.UserId);

        await _deleteUserService.DeleteUserAsync(request.UserId, ct);
    }
}