namespace etl_backend.Application.Abstraction;

public interface IDeleteUserService
{
    Task ExecuteAsync(string userId, CancellationToken cancellationToken);
}
