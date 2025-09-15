using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

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