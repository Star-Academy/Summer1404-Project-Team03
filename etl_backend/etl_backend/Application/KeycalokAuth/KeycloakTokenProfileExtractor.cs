using System.Security.Claims;
using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Dtos;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.KeycalokAuth;

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
        string? firstName = user.FindFirst("givenname")?.Value
                     ?? user.FindFirst(c => c.Type.EndsWith("givenname"))?.Value;
        string? lastName = user.FindFirst("surname")?.Value
                     ?? user.FindFirst(c => c.Type.EndsWith("surname"))?.Value;
        
        return new UserWithRolesDto
        {
            Id = id!,
            Username = user.FindFirst("preferred_username")?.Value!,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Roles = roles
        };
    }
}