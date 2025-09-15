using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbConfig.Abstraction;

public interface ISchemaDbContext : ICommonDbContext
{
    DbSet<DataTableSchema> DataTableSchemas { get; set; }
    DbSet<DataTableColumn> DataTableColumns { get; set; }
}