using Application.Common.Exceptions;
using Application.Dtos;
using Application.Services.Abstractions;
using Application.Users.Commands;
using Application.Users.CreateUser.ServiceAbstractions;
using MediatR;

namespace Application.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly ICreateUser _createUser;

    public CreateUserCommandHandler(ICreateUser createUser)
    {
        _createUser = createUser;
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

        var createdUser = await _createUser.CreateUserAsync(newUser, ct);
        return createdUser;
    }
}