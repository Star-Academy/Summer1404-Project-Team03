namespace etl_backend.Services.Auth.Abstraction;

public interface IRoleMapper
{
    IEnumerable<string> MapRolesFromClaim(string claimJson, string rolesKey = "roles");
}