namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakLogOutUser
{
    Task<bool> LogOutAsynk(string userId, CancellationToken ct = default);
}