using etl_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace etl_backend.DbConfig.Abstraction;

public interface ISchemaDbContext : ICommonDbContext
{
    DbSet<DataTableSchema> DataTableSchemas { get; set; }
    DbSet<DataTableColumn> DataTableColumns { get; set; }
}