using System.Security.Claims;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface ITokenProfileExtractor
{
    Task<UserWithRolesDto> ExtractProfile(ClaimsPrincipal user);
}