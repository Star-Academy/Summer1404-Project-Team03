namespace Infrastructure.DbConfig.Abstraction;

public interface ICommonDbContext : IAsyncDisposable, IDisposable
{
    public int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
}