namespace Infrastructure.DbConfig.Abstraction;

public interface IEtlDbContextFactory
{
    IStagingDbContext CreateStagingDbContext();
    ISchemaDbContext  CreateSchemaDbContext();
    IWorkflowDbContext CreateWorkflowDbContext();
}