namespace etl_backend.Application.UsersAuth.Abstraction;

public interface IDeleteUserService
{
    Task ExecuteAsync(string userId, CancellationToken cancellationToken);
}
