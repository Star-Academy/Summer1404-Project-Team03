using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Infrastructure.DbConfig.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbConfig;

[ExcludeFromCodeCoverage]
public class EtlDbContext : DbContext,  IStagingDbContext, ISchemaDbContext, IWorkflowDbContext
{
    public EtlDbContext(DbContextOptions<EtlDbContext> options)
        : base(options)
    {
    }

    public DbSet<DataTableSchema> DataTableSchemas { get; set; } = null!;
    public DbSet<DataTableColumn> DataTableColumns { get; set; } = null!; 
    public DbSet<StagedFile>  StagedFiles { get; set; } =  null!;
    public DbSet<Workflow> Workflows { get; set; } = null!;
    public DbSet<Plugin> Plugins { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new DataTableSchemaConfig());
        modelBuilder.ApplyConfiguration(new DataTableColumnConfig());
        modelBuilder.ApplyConfiguration(new StagedFileConfig());
        modelBuilder.ApplyConfiguration(new WorkflowConfig());
        modelBuilder.ApplyConfiguration(new PluginDbConfig());
    }
    
}
