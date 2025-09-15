namespace Infrastructure.SsoServices.User.Abstractions;

public interface IKeycloakLogOutUser
{
    Task<bool> LogOutAsynk(string userId, CancellationToken ct = default);
}