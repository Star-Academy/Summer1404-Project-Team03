using System.Security.Claims;
using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface ITokenProfileExtractor
{
    Task<UserWithRolesDto> ExtractProfile(ClaimsPrincipal user);
}