using System.Security.Claims;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenProfileExtractor
{
    Task<UserWithRolesDto> ExtractProfile(ClaimsPrincipal user);
}