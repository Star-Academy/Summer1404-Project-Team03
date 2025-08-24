namespace etl_backend.Services.Abstraction.Admin;

public interface IDeleteUserService
{
    Task ExecuteAsync(string userId, CancellationToken cancellationToken);
}
