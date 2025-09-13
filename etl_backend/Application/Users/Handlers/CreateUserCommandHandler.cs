using Application.Common.Exceptions;
using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.Commands;
using MediatR;

namespace Application.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserManagementService _userManagementService;

    public CreateUserCommandHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new UnprocessableEntityException("Username is required.");

        var newUser = new UserCreateDto
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password
        };

        var createdUser = await _userManagementService.CreateUserAsync(newUser, ct);
        return createdUser;
    }
}