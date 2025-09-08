using System.Security.Claims;
using Infrastructure.Dtos;

namespace Infrastructure.Identity.Abstractions;

public interface ITokenProfileExtractor
{
    Task<UserWithRolesDto> ExtractProfile(ClaimsPrincipal user);
}