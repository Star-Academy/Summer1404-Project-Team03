using Application.Dtos;
using Application.Users.ListUsers;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class GetAllUsersService : IListUsersService
{
    private readonly ISsoClient _ssoClient;
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public GetAllUsersService(
        ISsoClient ssoClient,
        IRoleManagerService roleManager,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 
    public async Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var usersJsonDoc = await _ssoClient.GetAsync(UsersEndpoint, accessToken!, cancellationToken);

        var users = usersJsonDoc.RootElement
            .EnumerateArray()
            .Select(UserMapper.FromJsonElement)
            .ToList();

        var result = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);
            result.Add(UserMapper.ToUserWithRolesDto(user, roles));
        }

        return result;
    }
}