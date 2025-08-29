using etl_backend.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace etl_backend.DbConfig;

public sealed class EtlDbContextFactory : IEtlDbContextFactory
{
    private readonly IDbContextFactory<EtlDbContext> _dbFactory;
    public EtlDbContextFactory(IDbContextFactory<EtlDbContext> dbFactory)
        => _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));

    public IStagingDbContext CreateStagingDbContext() => _dbFactory.CreateDbContext();
    public ISchemaDbContext  CreateSchemaDbContext()  => _dbFactory.CreateDbContext();
}
