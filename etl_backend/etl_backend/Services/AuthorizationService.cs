using System.Security.Claims;

namespace etl_backend.Services;

public interface IAuthorizationService
{
    bool HasRole(ClaimsPrincipal user, params string[] roles);
}

public class AuthorizationService : IAuthorizationService
{
    public bool HasRole(ClaimsPrincipal user, params string[] roles)
    {
        
        foreach (var role in roles)
        {
            
            if (user.IsInRole(role))
            {
                Console.WriteLine(role);
                return true;
            }
                
        }
        return false;
    }
}