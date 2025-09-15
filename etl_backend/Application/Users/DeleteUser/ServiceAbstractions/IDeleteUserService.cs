namespace Application.Users.DeleteUser.ServiceAbstractions;

public interface IDeleteUserService
{
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);
}