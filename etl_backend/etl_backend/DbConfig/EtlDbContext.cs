using System.Diagnostics.CodeAnalysis;
using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain;
using Microsoft.EntityFrameworkCore;
using etl_backend.DbConfig.Configurations;
using etl_backend.Domain.Entities;

namespace etl_backend.DbConfig;

[ExcludeFromCodeCoverage]
public class EtlDbContext : DbContext,  IStagingDbContext, ISchemaDbContext
{
    public EtlDbContext(DbContextOptions<EtlDbContext> options)
        : base(options)
    {
    }

    public DbSet<DataTableSchema> DataTableSchemas { get; set; } = null!;
    public DbSet<DataTableColumn> DataTableColumns { get; set; } = null!; 
    public DbSet<StagedFile>  StagedFiles { get; set; } =  null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DataTableSchemaConfig());
        modelBuilder.ApplyConfiguration(new DataTableColumnConfig());
        modelBuilder.ApplyConfiguration(new StagedFileConfig());
    }
}
