namespace etl_backend.DbConfig.Abstraction;

public interface IEtlDbContextFactory
{
    IStagingDbContext CreateStagingDbContext();
    ISchemaDbContext  CreateSchemaDbContext();
}