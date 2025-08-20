namespace etl_backend.Services.Abstraction.Admin;


public interface IGetUserByIdService
{
    Task<UserWithRolesDto> ExecuteAsync(string userId, string accessToken, CancellationToken cancellationToken);
}