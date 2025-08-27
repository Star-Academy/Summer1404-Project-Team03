using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

public class CreateUserService : ICreateUserService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public CreateUserService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<UserDto> ExecuteAsync(UserCreateDto newUser, CancellationToken cancellationToken)
    {
        
        return await _keycloakAdminClient.CreateUserAsync(newUser, cancellationToken);
    }
}