using Application.Common.Exceptions;
using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.Commands;
using Application.Users.EditUser.ServiceAbstractions;
using Application.Users.GetUserById.ServiceAbstractions;
using MediatR;

namespace Application.Users.Handlers;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, UserDto>
{
    private readonly IAdminEditUserService _adminEditUserService;
    private readonly IGetUserByIdService _getUserByIdService;

    public EditUserCommandHandler(IAdminEditUserService adminEditUserService, IGetUserByIdService getUserByIdService)
    {
        _adminEditUserService = adminEditUserService;
        _getUserByIdService = getUserByIdService;
    }

    public async Task<UserDto> Handle(EditUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            throw new UnprocessableEntityException("UserId is required.");

        // Optional: Validate user exists
        var existingUser = await _getUserByIdService.GetUserByIdAsync(request.UserId, ct);
        if (existingUser == null)
            throw new NotFoundException("User", request.UserId);

        var editRequest = new EditUserRequestDto
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var updatedUser = await _adminEditUserService.EditUserAsync(request.UserId, editRequest, ct);
        return updatedUser;
    }
}