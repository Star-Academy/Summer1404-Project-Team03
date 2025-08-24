using System.Security.Claims;
using etl_backend.Configuration;
using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Dtos;
using Microsoft.Extensions.Options;


namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakTokenProfileExtractor : ITokenProfileExtractor
{
    private readonly IRoleExtractor _roleExtractor;
    private readonly KeycloakOptions _options;

    public KeycloakTokenProfileExtractor(IRoleExtractor roleExtractor, IOptions<KeycloakOptions> options)
    {
        _roleExtractor = roleExtractor;
        _options = options.Value;
    }

    public async Task<UserWithRolesDto> ExtractProfile(ClaimsPrincipal user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var roles = await _roleExtractor.ExtractRoles(user, _options.RoleScope, _options.RolesKey);

        
        string? email = user.FindFirst("email")?.Value
                        ?? user.FindFirst(c => c.Type.EndsWith("emailaddress"))?.Value;
        string? id = user.FindFirst("id")?.Value
                     ?? user.FindFirst(c => c.Type.EndsWith("nameidentifier"))?.Value;
        return new UserWithRolesDto
        {
            Id = id!,
            Username = user.FindFirst("preferred_username")?.Value!,
            Email = email,
            FirstName = user.FindFirst("given_name")?.Value,
            LastName = user.FindFirst("family_name")?.Value,
            Roles = roles
        };
    }
}