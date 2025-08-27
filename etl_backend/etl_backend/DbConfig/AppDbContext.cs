using System.Diagnostics.CodeAnalysis;
using etl_backend.Domain;
using Microsoft.EntityFrameworkCore;
using etl_backend.DbConfig.Configurations;
using etl_backend.Domain.Entities;

namespace etl_backend.DbConfig;

[ExcludeFromCodeCoverage]
public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DataTableSchema> DataTableSchema { get; set; }
    public DbSet<DataTableColumn> DataTableColumns { get; set; }
    public DbSet<StagedFile>  StagedFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DataTableSchemaConfig());
        modelBuilder.ApplyConfiguration(new DataTableColumnConfig());
        modelBuilder.ApplyConfiguration(new StagedFileConfig());
    }
}
